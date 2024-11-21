using Authentication.Identity;
using Authentication.Token;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationTests.Token
{
    [TestFixture]
    internal class JwtHandlerTests
    {
        private JwtHandler jwtHandler;

        [SetUp]
        public void Setup()
        {
            var jwtSettings = new JwtSettings
            {
                Key = "this is super secret key for authentication testing",
                Issuer = "test_issuer",
                Audience = "test_audience",
                ExpiryInMinutes = 30
            };

            jwtHandler = new JwtHandler(jwtSettings);
        }

        [Test]
        public void CreateToken_ValidData_ValidAccessToken()
        {
            // Arrange
            var user = new IdentityUser
            {
                Email = "test@example.com",
                UserName = "testuser"
            };
            // Act
            var accessTokenData = jwtHandler.CreateToken(user, [Roles.CLIENT]);
            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(accessTokenData.AccessToken));
            Assert.IsFalse(string.IsNullOrEmpty(accessTokenData.RefreshToken));
        }
        [Test]
        public void GetPrincipalFromExpiredToken_ValidData_ValidPrincipal()
        {
            // Arrange
            var user = new IdentityUser
            {
                Email = "test@example.com",
                UserName = "testuser"
            };
            var accessTokenData = jwtHandler.CreateToken(user, new[] { Roles.CLIENT });
            // Act
            var principal = jwtHandler.GetPrincipalFromExpiredToken(accessTokenData.AccessToken);
            // Assert
            Assert.IsNotNull(principal);
            Assert.IsTrue(principal.Identity.IsAuthenticated);
        }
        [Test]
        public void GetPrincipalFromExpiredToken_InvalidData_ThrowsException()
        {
            // Arrange
            var invalidToken = "invalid_jwt_token";
            // Act & Assert
            Assert.Throws<SecurityTokenMalformedException>(() => jwtHandler.GetPrincipalFromExpiredToken(invalidToken));
        }
    }
}