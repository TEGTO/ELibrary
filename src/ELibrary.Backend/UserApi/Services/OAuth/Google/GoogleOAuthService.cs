using Authentication.Models;
using Authentication.OAuth.Google;
using UserApi.Domain.Models;
using UserEntities.Domain.Entities;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace UserApi.Services.OAuth.Google
{
    public class GoogleOAuthService : IOAuthService
    {
        private readonly IGoogleOAuthHttpClient httpClient;
        private readonly IUserOAuthCreationService userOAuthCreation;
        private readonly ITokenService tokenService;
        private readonly IGoogleTokenValidator googleTokenValidator;
        private readonly GoogleOAuthSettings oAuthSettings;
        private readonly double expiryInDays;

        public GoogleOAuthService(
            IGoogleOAuthHttpClient httpClient,
            IUserOAuthCreationService userOAuthCreation,
            ITokenService tokenService,
            IGoogleTokenValidator googleTokenValidator,
            GoogleOAuthSettings oAuthSettings,
            IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.userOAuthCreation = userOAuthCreation;
            this.tokenService = tokenService;
            this.googleTokenValidator = googleTokenValidator;
            this.oAuthSettings = oAuthSettings;
            expiryInDays = double.Parse(configuration[Configuration.AUTH_REFRESH_TOKEN_EXPIRY_IN_DAYS]!);
        }

        public async Task<AccessTokenData> GetAccessOnCodeAsync(GetAccessOnCodeParams accessOnCodeParams, CancellationToken cancellationToken)
        {
            var tokenResult = await httpClient.ExchangeAuthorizationCodeAsync
                (accessOnCodeParams.Code, accessOnCodeParams.CodeVerifier, accessOnCodeParams.RedirectUrl, cancellationToken);

            Payload payload = new();

            payload = await googleTokenValidator.ValidateAsync(tokenResult?.IdToken ?? "", new ValidationSettings
            {
                Audience = new[] { oAuthSettings.ClientId }
            });

            var userToBeCreated = new CreateUserFromOAuth
            {
                Email = payload.Email,
                LoginProviderSubject = payload.Subject,
                AuthMethod = AuthenticationMethod.GoogleOAuth
            };

            var refreshTokenExpiryDate = DateTime.UtcNow.AddDays(expiryInDays);

            var user = await userOAuthCreation.CreateUserFromOAuthAsync(userToBeCreated, cancellationToken);

            if (user == null) throw new InvalidOperationException("Failed to register user via oauth!");

            var tokenData = await tokenService.CreateNewTokenDataAsync(user, refreshTokenExpiryDate, cancellationToken);
            await tokenService.SetRefreshTokenAsync(user, tokenData, cancellationToken);

            return tokenData;
        }
        public string GenerateOAuthRequestUrl(GenerateOAuthRequestUrlParams generateUrlParams)
        {
            return httpClient.GenerateOAuthRequestUrl(oAuthSettings.Scope, generateUrlParams.RedirectUrl, generateUrlParams.CodeVerifier);
        }
    }
}
