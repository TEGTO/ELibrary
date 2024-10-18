using Authentication.Models;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Moq;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Requests;
using UserApi.Services;
using UserEntities.Domain.Entities;

namespace UserApi.Command.Client.LoginUser.Tests
{
    [TestFixture]
    internal class LoginUserCommandHandlerTests
    {
        private Mock<IMapper> mapperMock;
        private Mock<IAuthService> authServiceMock;
        private Mock<IConfiguration> configurationMock;
        private LoginUserCommandHandler loginUserCommandHandler;

        [SetUp]
        public void SetUp()
        {
            mapperMock = new Mock<IMapper>();
            authServiceMock = new Mock<IAuthService>();
            configurationMock = new Mock<IConfiguration>();

            configurationMock.Setup(c => c[It.Is<string>(s => s == Configuration.AUTH_REFRESH_TOKEN_EXPIRY_IN_DAYS)])
                             .Returns("7");

            loginUserCommandHandler = new LoginUserCommandHandler(authServiceMock.Object, mapperMock.Object, configurationMock.Object);
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
            authServiceMock.Setup(a => a.GetUserByUserInfoAsync(loginRequest.Login)).ReturnsAsync(user);
            authServiceMock.Setup(a => a.LoginUserAsync(It.IsAny<LoginUserParams>())).ReturnsAsync(tokenData);
            mapperMock.Setup(m => m.Map<AuthToken>(tokenData)).Returns(authToken);
            authServiceMock.Setup(a => a.GetUserRolesAsync(user)).ReturnsAsync(roles);
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
            authServiceMock.Setup(a => a.GetUserByUserInfoAsync(loginRequest.Login)).ReturnsAsync((User)null);
            // Act & Assert
            Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await loginUserCommandHandler.Handle(new LoginUserCommand(loginRequest), CancellationToken.None));
        }
    }
}