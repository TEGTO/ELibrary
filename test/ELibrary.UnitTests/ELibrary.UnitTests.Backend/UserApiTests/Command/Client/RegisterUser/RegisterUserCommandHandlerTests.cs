using Authentication.Models;
using AutoMapper;
using ExceptionHandling;
using Microsoft.AspNetCore.Identity;
using Moq;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Requests;
using UserApi.Services;
using UserApi.Services.Auth;
using UserEntities.Domain.Entities;

namespace UserApi.Command.Client.RegisterUser.Tests
{
    [TestFixture]
    internal class RegisterUserCommandHandlerTests
    {
        private Mock<IAuthService> authServiceMock;
        private Mock<IUserService> userServiceMock;
        private Mock<IMapper> mapperMock;
        private RegisterUserCommandHandler registerUserCommandHandler;

        [SetUp]
        public void SetUp()
        {
            mapperMock = new Mock<IMapper>();
            authServiceMock = new Mock<IAuthService>();
            userServiceMock = new Mock<IUserService>();

            registerUserCommandHandler = new RegisterUserCommandHandler(authServiceMock.Object, userServiceMock.Object, mapperMock.Object);
        }

        [Test]
        public async Task Handle_ValidRequest_ReturnsUserAuthenticationResponse()
        {
            // Arrange
            var registrationRequest = new UserRegistrationRequest { Email = "testuser@example.com", Password = "Password123" };
            var user = new User { Email = "testuser@example.com" };
            var tokenData = new AccessTokenData { AccessToken = "token", RefreshToken = "refreshToken" };
            var authToken = new AuthToken { AccessToken = "token", RefreshToken = "refreshToken" };
            var roles = new List<string> { "User" };
            mapperMock.Setup(m => m.Map<User>(registrationRequest)).Returns(user);
            authServiceMock.Setup(a => a.RegisterUserAsync(It.IsAny<RegisterUserParams>(), It.IsAny<CancellationToken>())).ReturnsAsync(IdentityResult.Success);
            userServiceMock.Setup(a => a.SetUserRolesAsync(It.IsAny<User>(), It.IsAny<List<string>>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<IdentityError>());
            authServiceMock.Setup(a => a.LoginUserAsync(It.IsAny<LoginUserParams>(), It.IsAny<CancellationToken>())).ReturnsAsync(tokenData);
            mapperMock.Setup(m => m.Map<AuthToken>(tokenData)).Returns(authToken);
            userServiceMock.Setup(a => a.GetUserRolesAsync(user, CancellationToken.None)).ReturnsAsync(roles);
            // Act
            var result = await registerUserCommandHandler.Handle(new RegisterUserCommand(registrationRequest), CancellationToken.None);
            // Assert
            Assert.That(result.Email, Is.EqualTo("testuser@example.com"));
            Assert.That(result.AuthToken.AccessToken, Is.EqualTo("token"));
            Assert.That(result.Roles, Is.EqualTo(roles));
        }
        [Test]
        public void Handle_FailedRegistration_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var registrationRequest = new UserRegistrationRequest { Email = "testuser@example.com", Password = "Password123" };
            var user = new User { Email = "testuser@example.com" };
            var errors = new List<IdentityError> { new IdentityError { Description = "Email already exists" } };
            mapperMock.Setup(m => m.Map<User>(registrationRequest)).Returns(user);
            authServiceMock.Setup(a => a.RegisterUserAsync(It.IsAny<RegisterUserParams>(), It.IsAny<CancellationToken>())).ReturnsAsync(IdentityResult.Failed(errors.ToArray()));
            // Act & Assert
            var ex = Assert.ThrowsAsync<AuthorizationException>(async () => await registerUserCommandHandler.Handle(new RegisterUserCommand(registrationRequest), CancellationToken.None));
            Assert.That(ex.Errors.First(), Is.EqualTo("Email already exists"));
        }
    }
}