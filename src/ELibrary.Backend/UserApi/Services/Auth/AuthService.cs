using Authentication.Models;
using Microsoft.AspNetCore.Identity;
using UserEntities.Domain.Entities;

namespace UserApi.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> userManager;
        private readonly ITokenService tokenService;
        private readonly double expiryInDays;

        public AuthService(UserManager<User> userManager, ITokenService tokenService, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
            expiryInDays = double.Parse(configuration[Configuration.AUTH_REFRESH_TOKEN_EXPIRY_IN_DAYS]!);
        }

        #region IAuthService Members

        public async Task<IdentityResult> RegisterUserAsync(RegisterUserParams registerParams, CancellationToken cancellationToken)
        {
            registerParams.User.LoginProvider = LoginProvider.BaseAuthentication;
            return await userManager.CreateAsync(registerParams.User, registerParams.Password);
        }
        public async Task<AccessTokenData> LoginUserAsync(LoginUserParams loginParams, CancellationToken cancellationToken)
        {
            var user = await GetUserByLoginAsync(loginParams.Login);

            if (user == null || !await userManager.CheckPasswordAsync(user, loginParams.Password))
            {
                throw new UnauthorizedAccessException("Invalid authentication. Check Login or password.");
            }

            var refreshTokenExpiryDate = DateTime.UtcNow.AddDays(expiryInDays);

            var tokenData = await tokenService.CreateNewTokenDataAsync(user, refreshTokenExpiryDate, cancellationToken);
            await tokenService.SetRefreshTokenAsync(user, tokenData, cancellationToken);

            return tokenData;
        }
        public async Task<AccessTokenData> RefreshTokenAsync(AccessTokenData accessTokenData, CancellationToken cancellationToken)
        {
            var principal = tokenService.GetPrincipalFromExpiredToken(accessTokenData.AccessToken);
            var user = await userManager.FindByNameAsync(principal.Identity.Name);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid authentication. AccessToken is not valid.");
            }

            if (user.RefreshToken != accessTokenData.RefreshToken || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                throw new InvalidDataException("Refresh token is not valid!");
            }

            var refreshTokenExpiryDate = DateTime.UtcNow.AddDays(expiryInDays);

            var tokenData = await tokenService.CreateNewTokenDataAsync(user, refreshTokenExpiryDate, cancellationToken);
            await tokenService.SetRefreshTokenAsync(user, tokenData, cancellationToken);

            return tokenData;
        }

        #endregion

        #region Private Helpers

        private async Task<User?> GetUserByLoginAsync(string login)
        {
            var user = await userManager.FindByEmailAsync(login);
            user = user == null ? await userManager.FindByNameAsync(login) : user;
            return user;
        }

        #endregion
    }
}