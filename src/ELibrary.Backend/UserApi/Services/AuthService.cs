using Authentication.Models;
using Authentication.Services;
using Microsoft.AspNetCore.Identity;
using UserEntities.Domain.Entities;

namespace UserApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> userManager;
        private readonly ITokenHandler tokenHandler;

        public AuthService(UserManager<User> userManager, ITokenHandler tokenHandler)
        {
            this.userManager = userManager;
            this.tokenHandler = tokenHandler;
        }

        #region IAuthService Members

        public async Task<IdentityResult> RegisterUserAsync(RegisterUserParams registerParams)
        {
            return await userManager.CreateAsync(registerParams.User, registerParams.Password);
        }
        public async Task<AccessTokenData> LoginUserAsync(LoginUserParams loginParams)
        {
            var user = await GetUserByLoginAsync(loginParams.Login);

            if (user == null || !await userManager.CheckPasswordAsync(user, loginParams.Password))
            {
                throw new UnauthorizedAccessException("Invalid authentication. Check Login or password.");
            }

            var tokenData = await CreateNewTokenDataAsync(user, loginParams.RefreshTokenExpiryInDays);
            await SetRefreshToken(user, tokenData);

            return tokenData;
        }
        public async Task<AccessTokenData> RefreshTokenAsync(AccessTokenData accessTokenData, double refreshTokenExpiryInDays)
        {
            var principal = tokenHandler.GetPrincipalFromExpiredToken(accessTokenData.AccessToken);
            var user = await userManager.FindByNameAsync(principal.Identity.Name);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid authentication. AccessToken is not valid.");
            }

            if (user.RefreshToken != accessTokenData.RefreshToken || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                throw new InvalidDataException("Refresh token is not valid!");
            }

            var tokenData = await CreateNewTokenDataAsync(user, refreshTokenExpiryInDays);
            await SetRefreshToken(user, tokenData);

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
        private async Task<AccessTokenData> CreateNewTokenDataAsync(User user, double refreshTokenExpiryInDays)
        {
            var roles = await userManager.GetRolesAsync(user);
            var tokenData = tokenHandler.CreateToken(user, roles);
            tokenData.RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(refreshTokenExpiryInDays);
            return tokenData;
        }
        private async Task SetRefreshToken(User user, AccessTokenData accessTokenData)
        {
            user.RefreshToken = accessTokenData.RefreshToken;
            user.RefreshTokenExpiryTime = accessTokenData.RefreshTokenExpiryDate;
            await userManager.UpdateAsync(user);
        }

        #endregion
    }
}