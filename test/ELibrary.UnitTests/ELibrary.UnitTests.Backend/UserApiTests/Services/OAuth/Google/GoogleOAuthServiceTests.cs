using Authentication.Models;
using Authentication.OAuth.Google;
using Microsoft.Extensions.Configuration;
using Moq;
using UserApi.Domain.Models;
using UserEntities.Domain.Entities;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace UserApi.Services.OAuth.Google.Tests
{
    [TestFixture]
    internal class GoogleOAuthServiceTests
    {
        private Mock<IGoogleOAuthHttpClient> mockHttpClient;
        private Mock<IUserOAuthCreationService> mockUserOAuthCreation;
        private Mock<ITokenService> mockTokenService;
        private Mock<IGoogleTokenValidator> mockGoogleTokenValidator;
        private Mock<IConfiguration> mockConfiguration;
        private GoogleOAuthSettings mockOAuthSettings;
        private GoogleOAuthService googleOAuthService;

        [SetUp]
        public void SetUp()
        {
            mockHttpClient = new Mock<IGoogleOAuthHttpClient>();

            mockUserOAuthCreation = new Mock<IUserOAuthCreationService>();

            mockTokenService = new Mock<ITokenService>();
            mockGoogleTokenValidator = new Mock<IGoogleTokenValidator>();

            mockOAuthSettings = new GoogleOAuthSettings
            {
                ClientId = "test-client-id",
                Scope = "https://www.googleapis.com/auth/userinfo.email"
            };

            mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(config => config[Configuration.AUTH_REFRESH_TOKEN_EXPIRY_IN_DAYS])
                .Returns("7");

            googleOAuthService = new GoogleOAuthService(
                mockHttpClient.Object,
                mockUserOAuthCreation.Object,
                mockTokenService.Object,
                mockGoogleTokenValidator.Object,
                mockOAuthSettings,
                mockConfiguration.Object);
        }

        [Test]
        public async Task GetAccessOnCodeAsync_ValidParams_ReturnsAccessTokenData()
        {
            // Arrange
            var accessOnCodeParams = new GetAccessOnCodeParams("valid-code", "code-verifier", "https://example.com/callback");
            var tokenResult = new GoogleOAuthTokenResult
            {
                AccessToken = "access-token",
                RefreshToken = "refresh-token",
                IdToken = "id-token"
            };
            var user = new User { UserName = "testuser" };

            var expectedAccessTokenData = new AccessTokenData
            {
                AccessToken = "access-token",
                RefreshToken = "refresh-token",
                RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(7)
            };

            var mockPayload = new Payload
            {
                Email = "test@example.com",
                Subject = "1234567890",
                Audience = "test-client-id"
            };

            mockHttpClient.Setup(client => client.ExchangeAuthorizationCodeAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tokenResult);
            mockUserOAuthCreation.Setup(x => x.CreateUserFromOAuthAsync(It.IsAny<CreateUserFromOAuth>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            mockTokenService.Setup(service => service.CreateNewTokenDataAsync(user, It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedAccessTokenData);
            mockGoogleTokenValidator.Setup(x => x.ValidateAsync(It.IsAny<string>(), It.IsAny<ValidationSettings>()))
                .ReturnsAsync(mockPayload);
            // Act
            var result = await googleOAuthService.GetAccessOnCodeAsync(accessOnCodeParams, CancellationToken.None);
            // Assert
            Assert.NotNull(result);
            Assert.That(result.AccessToken, Is.EqualTo(expectedAccessTokenData.AccessToken));
            Assert.That(result.RefreshToken, Is.EqualTo(expectedAccessTokenData.RefreshToken));
        }

        [Test]
        public void GenerateOAuthRequestUrl_ValidParams_ReturnsCorrectUrl()
        {
            // Arrange
            var generateUrlParams = new GenerateOAuthRequestUrlParams("https://example.com/callback", "code-verifier");
            // Act
            googleOAuthService.GenerateOAuthRequestUrl(generateUrlParams);
            // Assert
            mockHttpClient.Verify(x => x.GenerateOAuthRequestUrl(
               mockOAuthSettings.Scope,
               generateUrlParams.RedirectUrl,
               generateUrlParams.CodeVerifier), Times.Once);
        }
    }
}