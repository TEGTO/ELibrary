using Authentication.Models;
using AuthenticationApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationApi.Services
{
    public interface IAuthService
    {
        public Task<AccessTokenData> LoginUserAsync(string login, string password, double refreshTokenExpiryInDays);
        public Task<User?> GetUserByLoginAsync(string login);
        public Task<AccessTokenData> RefreshTokenAsync(AccessTokenData accessTokenData, double refreshTokenExpiryInDays);
        public Task<IdentityResult> RegisterUserAsync(User user, string password);
    }
}