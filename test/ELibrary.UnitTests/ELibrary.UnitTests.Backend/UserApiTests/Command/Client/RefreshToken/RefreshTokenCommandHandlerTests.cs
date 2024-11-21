using Authentication.Models;
using AutoMapper;
using Moq;
using System.Security.Claims;
using UserApi.Domain.Dtos;
using UserApi.Services;
using UserApi.Services.Auth;
using UserEntities.Domain.Entities;

namespace UserApi.Command.Client.RefreshToken.Tests
{
    [TestFixture]
    internal class RefreshTokenCommandHandlerTests
    {
        private Mock<IAuthService> authServiceMock;
        private Mock<IUserService> userServiceMock;
        private Mock<ITokenService> tokenServiceMock;
        private Mock<IMapper> mapperMock;
        private RefreshTokenCommandHandler refreshTokenCommandHandler;

        [SetUp]
        public void SetUp()
        {
            mapperMock = new Mock<IMapper>();
            userServiceMock = new Mock<IUserService>();
            authServiceMock = new Mock<IAuthService>();
            tokenServiceMock = new Mock<ITokenService>();

            refreshTokenCommandHandler = new RefreshTokenCommandHandler(
                authServiceMock.Object,
                userServiceMock.Object,
                tokenServiceMock.Object,
                mapperMock.Object);
        }
        [Test]
        public async Task Handle_ValidRequest_ReturnsNewAuthToken()
        {
            // Arrange
            var token = new AuthToken { AccessToken = "oldToken", RefreshToken = "oldRefreshToken" };
            var tokenData = new AccessTokenData { AccessToken = "newToken", RefreshToken = "newRefreshToken" };
            var newAuthToken = new AuthToken { AccessToken = "newToken", RefreshToken = "newRefreshToken" };
            mapperMock.Setup(m => m.Map<AccessTokenData>(token)).Returns(tokenData);
            authServiceMock.Setup(a => a.RefreshTokenAsync(It.IsAny<RefreshTokenParams>(), It.IsAny<CancellationToken>())).ReturnsAsync(tokenData);
            mapperMock.Setup(m => m.Map<AuthToken>(tokenData)).Returns(newAuthToken);
            userServiceMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new User());
            // Act
            var result = await refreshTokenCommandHandler.Handle(new RefreshTokenCommand(token), CancellationToken.None);
            // Assert
            Assert.That(result.AccessToken, Is.EqualTo("newToken"));
            Assert.That(result.RefreshToken, Is.EqualTo("newRefreshToken"));
        }
    }
}