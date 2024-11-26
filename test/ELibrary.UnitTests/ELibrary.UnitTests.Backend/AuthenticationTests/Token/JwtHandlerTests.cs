using Authentication.Identity;
using Authentication.Token;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

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
        [TestCase("test@example.com", "testuser", new[] { Roles.ADMINISTRATOR })]
        [TestCase("", "testuser", new[] { Roles.ADMINISTRATOR })]
        [TestCase("", "", new[] { Roles.ADMINISTRATOR })]
        [TestCase("user@example.com", "user123", new[] { Roles.CLIENT, Roles.MANAGER })]
        [TestCase("admin@example.com", "admin", new string[0])]
        [TestCase("", "admin", new string[0])]
        [TestCase("", "", new string[0])]
        public void CreateToken_ValidData_ShouldReturnValidAccessToken(string email, string username, string[] roles)
        {
            // Arrange
            var user = new IdentityUser
            {
                Email = email,
                UserName = username
            };

            // Act
            var accessTokenData = jwtHandler.CreateToken(user, roles);

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(accessTokenData.AccessToken));
            Assert.IsFalse(string.IsNullOrEmpty(accessTokenData.RefreshToken));

            var tokenHandler = new JwtSecurityTokenHandler();
            Assert.IsTrue(tokenHandler.CanReadToken(accessTokenData.AccessToken));
        }
        [TestCase(null, null, null)]
        public void CreateToken_NullData_ThrowsNullReferenceException(string email, string username, string[] roles)
        {
            // Arrange
            var user = new IdentityUser
            {
                Email = email,
                UserName = username
            };

            // Act + Assert
            Assert.Throws<NullReferenceException>(() => jwtHandler.CreateToken(user, roles));
        }

        [Test]
        [TestCase("test@example.com", "testuser", new[] { Roles.ADMINISTRATOR })]
        [TestCase("", "testuser", new[] { Roles.ADMINISTRATOR })]
        [TestCase("user@example.com", "user123", new[] { Roles.CLIENT, Roles.MANAGER })]
        [TestCase("admin@example.com", "admin", new string[0])]
        [TestCase("", "admin", new string[0])]
        [TestCase("", "", new string[0])]
        public void GetPrincipalFromExpiredToken_ValidData_ValidPrincipal(string email, string username, string[] roles)
        {
            // Arrange
            var user = new IdentityUser
            {
                Email = email,
                UserName = username
            };
            var accessTokenData = jwtHandler.CreateToken(user, roles);

            // Act
            Assert.IsNotNull(accessTokenData.AccessToken);
            var principal = jwtHandler.GetPrincipalFromExpiredToken(accessTokenData.AccessToken);

            // Assert
            Assert.IsNotNull(principal);
            Assert.IsNotNull(principal.Identity);
            Assert.IsTrue(principal.Identity.IsAuthenticated);
        }
        [Test]
        [TestCase(null, typeof(ArgumentNullException))]
        [TestCase("", typeof(ArgumentNullException))]
        [TestCase("invalid_jwt_token", typeof(SecurityTokenMalformedException))]
        public void GetPrincipalFromExpiredToken_InvalidData_ThrowsException(string token, Type exceptionType)
        {
            // Act & Assert
            var exception = Assert.Throws(exceptionType, () => jwtHandler.GetPrincipalFromExpiredToken(token));
            Assert.IsInstanceOf(exceptionType, exception);
        }
    }
}