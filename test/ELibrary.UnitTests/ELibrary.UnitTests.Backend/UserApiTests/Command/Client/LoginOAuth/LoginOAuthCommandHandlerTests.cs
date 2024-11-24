using Authentication.Models;
using Authentication.OAuth;
using AutoMapper;
using Moq;
using System.Security.Claims;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Requests;
using UserApi.Services;
using UserApi.Services.Auth;
using UserApi.Services.OAuth;
using UserEntities.Domain.Entities;

namespace UserApi.Command.Client.LoginOAuth.Tests
{
    [TestFixture]
    internal class LoginOAuthCommandHandlerTests
    {
        private Mock<IUserService> mockUserService;
        private Mock<ITokenService> mockTokenService;
        private Mock<IOAuthService> mockOAuthService;
        private Mock<IAuthService> mockAuthService;
        private Mock<IMapper> mockMapper;
        private LoginOAuthCommandHandler handler;
        private Dictionary<OAuthLoginProvider, IOAuthService> oAuthServices;

        [SetUp]
        public void SetUp()
        {
            mockUserService = new Mock<IUserService>();
            mockTokenService = new Mock<ITokenService>();
            mockOAuthService = new Mock<IOAuthService>();
            mockAuthService = new Mock<IAuthService>();
            mockMapper = new Mock<IMapper>();

            oAuthServices = new Dictionary<OAuthLoginProvider, IOAuthService>
            {
                { OAuthLoginProvider.Google, mockOAuthService.Object }
            };

            handler = new LoginOAuthCommandHandler(oAuthServices, mockAuthService.Object, mockUserService.Object, mockTokenService.Object, mockMapper.Object);
        }

        [Test]
        public async Task Handle_ValidLogin_ReturnsAuthenticationResponse()
        {
            // Arrange
            var command = new LoginOAuthCommand
            (
                new LoginOAuthRequest
                {
                    OAuthLoginProvider = OAuthLoginProvider.Google,
                    Code = "valid_code",
                    CodeVerifier = "valid_verifier",
                    RedirectUrl = "https://localhost/callback"
                }
            );

            var accessToken = new AuthToken { AccessToken = "valid_access_token", RefreshToken = "valid_refresh_token" };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testuser@gmail.com")
            }));

            var user = new User { Email = "testuser@gmail.com" };
            var roles = new List<string> { "Admin" };

            var tokenDto = new AccessTokenData { AccessToken = "new_access_token", RefreshToken = "new_refresh_token" };

            mockOAuthService.Setup(s => s.GetAccessOnCodeAsync(It.IsAny<GetAccessOnCodeParams>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tokenDto);

            mockTokenService.Setup(s => s.GetPrincipalFromToken(tokenDto.AccessToken))
                .Returns(principal);

            mockMapper.Setup(m => m.Map<AuthToken>(tokenDto))
                .Returns(accessToken);

            mockUserService.Setup(s => s.GetUserByLoginAsync(principal.Identity!.Name!, CancellationToken.None))
                .ReturnsAsync(user);
            mockUserService.Setup(s => s.GetUserRolesAsync(user, CancellationToken.None))
                .ReturnsAsync(roles);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.Email, Is.EqualTo(user.Email));
            Assert.That(result.Roles, Is.EqualTo(roles));
            Assert.That(result.AuthToken, Is.EqualTo(accessToken));
        }

        [Test]
        public void Handle_UserNotFound_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var command = new LoginOAuthCommand
            (
               new LoginOAuthRequest
               {
                   OAuthLoginProvider = OAuthLoginProvider.Google,
                   Code = "valid_code",
                   CodeVerifier = "valid_verifier",
                   RedirectUrl = "https://localhost/callback"
               }
            );

            var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "nonexistentuser@gmail.com")
            }));

            var tokenDto = new AccessTokenData { AccessToken = "new_access_token", RefreshToken = "new_refresh_token" };

            mockOAuthService.Setup(s => s.GetAccessOnCodeAsync(It.IsAny<GetAccessOnCodeParams>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tokenDto);

            mockTokenService.Setup(s => s.GetPrincipalFromToken(tokenDto.AccessToken))
                .Returns(principal);

            mockUserService.Setup(s => s.GetUserByLoginAsync(principal.Identity!.Name!, CancellationToken.None))
                .ReturnsAsync(null as User);

            // Act & Assert
            Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await handler.Handle(command, CancellationToken.None));
        }
    }
}