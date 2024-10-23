using Authentication.Models;
using Authentication.OAuth.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserApi.Domain.Models;
using UserEntities.Data;
using UserEntities.Domain.Entities;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace UserApi.Services.OAuth.Google
{
    public class GoogleOAuthService : IOAuthService
    {
        private readonly IGoogleOAuthHttpClient httpClient;
        private readonly UserManager<User> userManager;
        private readonly IDbContextFactory<UserIdentityDbContext> dbContextFactory;
        private readonly ITokenService tokenService;
        private readonly GoogleOAuthSettings oAuthSettings;
        private readonly double expiryInDays;

        public GoogleOAuthService(
            IGoogleOAuthHttpClient httpClient,
            UserManager<User> userManager,
            IDbContextFactory<UserIdentityDbContext> dbContextFactory,
            ITokenService tokenService,
            GoogleOAuthSettings oAuthSettings,
            IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.userManager = userManager;
            this.dbContextFactory = dbContextFactory;
            this.tokenService = tokenService;
            this.oAuthSettings = oAuthSettings;
            expiryInDays = double.Parse(configuration[Configuration.AUTH_REFRESH_TOKEN_EXPIRY_IN_DAYS]!);
        }

        public async Task<AccessTokenData> GetAccessOnCodeAsync(GetAccessOnCodeParams accessOnCodeParams, CancellationToken cancellationToken)
        {
            var tokenResult = await httpClient.ExchangeAuthorizationCodeAsync
                (accessOnCodeParams.Code, accessOnCodeParams.CodeVerifier, accessOnCodeParams.RedirectUrl, cancellationToken);

            Payload payload = new();

            payload = await ValidateAsync(tokenResult.IdToken, new ValidationSettings
            {
                Audience = new[] { oAuthSettings.ClientId }
            });

            var userToBeCreated = new CreateUserFromOAuth
            {
                Email = payload.Email,
                LoginProviderSubject = payload.Subject,
                LoginProvider = LoginProvider.GoogleOAuth
            };

            var refreshTokenExpiryDate = DateTime.UtcNow.AddDays(expiryInDays);

            var user = await userManager.CreateUserFromOAuth(await dbContextFactory.CreateDbContextAsync(cancellationToken), userToBeCreated);

            var tokenData = await tokenService.CreateNewTokenDataAsync(user, refreshTokenExpiryDate, cancellationToken);
            await tokenService.SetRefreshTokenAsync(user, tokenData, cancellationToken);

            return tokenData;
        }
        public string GenerateOAuthRequestUrl(GenerateOAuthRequestUrlParams generateUrlParams)
        {
            return httpClient.GenerateOAuthRequestUrl(oAuthSettings.Scope, generateUrlParams.RedirectUrl, generateUrlParams.CodeVerifier);
        }
        public async Task<AccessTokenData> RefreshTokenAsync(AccessTokenData accessTokenData, CancellationToken cancellationToken)
        {
            var principal = tokenService.GetPrincipalFromExpiredToken(accessTokenData.AccessToken);
            var user = await userManager.FindByNameAsync(principal.Identity.Name);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid authentication. AccessToken is not valid.");
            }

            if (accessTokenData.RefreshToken == null ||
                user.RefreshToken != accessTokenData.RefreshToken ||
                user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                throw new InvalidDataException("Refresh token is not valid!");
            }

            var tokenResult = await httpClient.RefreshAccessTokenAsync(accessTokenData.RefreshToken, cancellationToken);

            var refreshTokenExpiryDate = DateTime.UtcNow.AddDays(expiryInDays);

            var tokenData = await tokenService.CreateNewTokenDataAsync(user, refreshTokenExpiryDate, cancellationToken);
            await tokenService.SetRefreshTokenAsync(user, tokenData, cancellationToken);

            return tokenData;
        }
    }
}
