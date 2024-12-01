using Authentication.Models;
using AutoMapper;
using ExceptionHandling;
using Microsoft.AspNetCore.Identity;
using Moq;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Requests;
using UserApi.Domain.Models;
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
        private Mock<IUserAuthenticationMethodService> authMethodServiceMock;
        private Mock<IMapper> mapperMock;
        private RegisterUserCommandHandler registerUserCommandHandler;

        [SetUp]
        public void SetUp()
        {
            mapperMock = new Mock<IMapper>();
            authServiceMock = new Mock<IAuthService>();
            userServiceMock = new Mock<IUserService>();
            authMethodServiceMock = new Mock<IUserAuthenticationMethodService>();

            registerUserCommandHandler = new RegisterUserCommandHandler(authServiceMock.Object, userServiceMock.Object, authMethodServiceMock.Object, mapperMock.Object);
        }

        [Test]
        public async Task Handle_ValidRequest_ReturnsUserAuthenticationResponse()
        {
            // Arrange
            var user = new User { Email = "testuser@example.com" };
            var roles = new List<string> { "User" };

            var tokenData = new AccessTokenData { AccessToken = "token", RefreshToken = "refreshToken" };
            var authToken = new AuthToken { AccessToken = "token", RefreshToken = "refreshToken" };
            var registrationRequest = new UserRegistrationRequest { Email = "testuser@example.com", Password = "Password123" };

            mapperMock.Setup(m => m.Map<User>(registrationRequest)).Returns(user);
            mapperMock.Setup(m => m.Map<AuthToken>(tokenData)).Returns(authToken);

            authServiceMock.Setup(a => a.RegisterUserAsync(It.IsAny<RegisterUserModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(IdentityResult.Success);
            authServiceMock.Setup(a => a.LoginUserAsync(It.IsAny<LoginUserModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(tokenData);

            userServiceMock.Setup(a => a.SetUserRolesAsync(It.IsAny<User>(), It.IsAny<List<string>>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<IdentityError>());
            userServiceMock.Setup(a => a.GetUserRolesAsync(user, CancellationToken.None)).ReturnsAsync(roles);

            // Act
            var result = await registerUserCommandHandler.Handle(new RegisterUserCommand(registrationRequest), CancellationToken.None);

            // Assert
            Assert.That(result.Email, Is.EqualTo("testuser@example.com"));

            Assert.IsNotNull(result.AuthToken);
            Assert.IsNotNull(result.AuthToken.AccessToken);

            Assert.That(result.AuthToken.AccessToken, Is.EqualTo("token"));
            Assert.That(result.Roles, Is.EqualTo(roles));
        }

        [Test]
        public void Handle_FailedRegistration_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var user = new User { Email = "testuser@example.com" };
            var errors = new List<IdentityError> { new IdentityError { Description = "Email already exists" } };

            var registrationRequest = new UserRegistrationRequest { Email = "testuser@example.com", Password = "Password123" };

            mapperMock.Setup(m => m.Map<User>(registrationRequest)).Returns(user);

            authServiceMock.Setup(a => a.RegisterUserAsync(It.IsAny<RegisterUserModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(IdentityResult.Failed(errors.ToArray()));

            // Act & Assert
            var ex = Assert.ThrowsAsync<AuthorizationException>(async () => await registerUserCommandHandler.Handle(new RegisterUserCommand(registrationRequest), CancellationToken.None));
            Assert.That(ex.Errors.First(), Is.EqualTo("Email already exists"));
        }
    }
}