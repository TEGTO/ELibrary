﻿using Authentication.Models;
using AutoMapper;
using Moq;
using UserApi.Domain.Dtos;
using UserApi.Services.Auth;

namespace UserApi.Command.Client.RefreshToken.Tests
{
    [TestFixture]
    internal class RefreshTokenCommandHandlerTests
    {
        private Mock<IAuthService> authServiceMock;
        private Mock<IMapper> mapperMock;
        private RefreshTokenCommandHandler refreshTokenCommandHandler;

        [SetUp]
        public void SetUp()
        {
            mapperMock = new Mock<IMapper>();
            authServiceMock = new Mock<IAuthService>();

            refreshTokenCommandHandler = new RefreshTokenCommandHandler(authServiceMock.Object, mapperMock.Object);
        }
        [Test]
        public async Task Handle_ValidRequest_ReturnsNewAuthToken()
        {
            // Arrange
            var token = new AuthToken { AccessToken = "oldToken", RefreshToken = "oldRefreshToken" };
            var tokenData = new AccessTokenData { AccessToken = "newToken", RefreshToken = "newRefreshToken" };
            var newAuthToken = new AuthToken { AccessToken = "newToken", RefreshToken = "newRefreshToken" };
            mapperMock.Setup(m => m.Map<AccessTokenData>(token)).Returns(tokenData);
            authServiceMock.Setup(a => a.RefreshTokenAsync(tokenData, It.IsAny<CancellationToken>())).ReturnsAsync(tokenData);
            mapperMock.Setup(m => m.Map<AuthToken>(tokenData)).Returns(newAuthToken);
            // Act
            var result = await refreshTokenCommandHandler.Handle(new RefreshTokenCommand(token), CancellationToken.None);
            // Assert
            Assert.That(result.AccessToken, Is.EqualTo("newToken"));
            Assert.That(result.RefreshToken, Is.EqualTo("newRefreshToken"));
        }
    }
}