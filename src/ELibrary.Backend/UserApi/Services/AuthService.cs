using Authentication.Models;
using Authentication.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using UserApi.Domain.Entities;

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

        public async Task<IdentityResult> RegisterUserAsync(User user, string password)
        {
            return await userManager.CreateAsync(user, password);
        }
        public async Task<AccessTokenData> LoginUserAsync(string login, string password, double refreshTokenExpiryInDays)
        {
            var user = await GetUserByLoginAsync(login);

            if (user == null || !await userManager.CheckPasswordAsync(user, password))
            {
                throw new UnauthorizedAccessException("Invalid authentication. Login is not correct.");
            }

            var tokenData = CreateNewTokenData(user, refreshTokenExpiryInDays);
            await SetRefreshToken(user, tokenData);

            return tokenData;
        }
        public async Task<User?> GetUserAsync(ClaimsPrincipal principal)
        {
            var name = principal.FindFirstValue(ClaimTypes.Name);
            return name.IsNullOrEmpty() ? null : await GetUserByLoginAsync(name);
        }
        public async Task<User?> GetUserByLoginAsync(string login)
        {
            var user = await userManager.FindByNameAsync(login);
            return user;
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

            var tokenData = CreateNewTokenData(user, refreshTokenExpiryInDays);
            await SetRefreshToken(user, tokenData);

            return tokenData;
        }

        #endregion

        #region Private Helpers

        private AccessTokenData CreateNewTokenData(User user, double refreshTokenExpiryInDays)
        {
            var tokenData = tokenHandler.CreateToken(user);
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