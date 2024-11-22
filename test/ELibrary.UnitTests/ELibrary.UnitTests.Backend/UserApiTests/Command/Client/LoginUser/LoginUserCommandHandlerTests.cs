using Authentication.Models;
using AutoMapper;
using Moq;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Requests;
using UserApi.Services;
using UserApi.Services.Auth;
using UserEntities.Domain.Entities;

namespace UserApi.Command.Client.LoginUser.Tests
{
    [TestFixture]
    internal class LoginUserCommandHandlerTests
    {
        private Mock<IAuthService> authServiceMock;
        private Mock<IUserService> userServiceMock;
        private Mock<IUserAuthenticationMethodService> authMethodServiceMock;
        private Mock<IMapper> mapperMock;
        private LoginUserCommandHandler loginUserCommandHandler;

        [SetUp]
        public void SetUp()
        {
            mapperMock = new Mock<IMapper>();
            authServiceMock = new Mock<IAuthService>();
            userServiceMock = new Mock<IUserService>();
            authMethodServiceMock = new Mock<IUserAuthenticationMethodService>();

            loginUserCommandHandler = new LoginUserCommandHandler(authServiceMock.Object, userServiceMock.Object, authMethodServiceMock.Object, mapperMock.Object);
        }

        [Test]
        public async Task Handle_ValidRequest_ReturnsUserAuthenticationResponse()
        {
            // Arrange
            var loginRequest = new UserAuthenticationRequest { Login = "testuser", Password = "Password123" };
            var user = new User { Email = "testuser@example.com" };
            var tokenData = new AccessTokenData { AccessToken = "token", RefreshToken = "refreshToken" };
            var authToken = new AuthToken { AccessToken = "token", RefreshToken = "refreshToken" };
            var roles = new List<string> { "User" };

            userServiceMock.Setup(a => a.GetUserByLoginAsync(loginRequest.Login, CancellationToken.None)).ReturnsAsync(user);
            userServiceMock.Setup(a => a.GetUserRolesAsync(user, CancellationToken.None)).ReturnsAsync(roles);

            authServiceMock.Setup(a => a.LoginUserAsync(It.IsAny<LoginUserParams>(), It.IsAny<CancellationToken>())).ReturnsAsync(tokenData);

            mapperMock.Setup(m => m.Map<AuthToken>(tokenData)).Returns(authToken);

            // Act
            var result = await loginUserCommandHandler.Handle(new LoginUserCommand(loginRequest), CancellationToken.None);

            // Assert
            Assert.That(result.Email, Is.EqualTo("testuser@example.com"));
            Assert.That(result.AuthToken.AccessToken, Is.EqualTo("token"));
            Assert.That(result.Roles, Is.EqualTo(roles));
        }
        [Test]
        public void Handle_InvalidCredentials_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var loginRequest = new UserAuthenticationRequest { Login = "invaliduser", Password = "wrongpassword" };

            userServiceMock.Setup(a => a.GetUserByLoginAsync(loginRequest.Login, CancellationToken.None)).ReturnsAsync((User)null);

            // Act & Assert
            Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await loginUserCommandHandler.Handle(new LoginUserCommand(loginRequest), CancellationToken.None));
        }
    }
}