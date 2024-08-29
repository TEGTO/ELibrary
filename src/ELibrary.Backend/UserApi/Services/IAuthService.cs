using Authentication.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using UserApi.Domain.Entities;

namespace UserApi.Services
{
    public interface IAuthService
    {
        public Task<AccessTokenData> LoginUserAsync(string login, string password, double refreshTokenExpiryInDays);
        public Task<User?> GetUserByLoginAsync(string login);
        public Task<User> GetUserAsync(ClaimsPrincipal user);
        public Task<AccessTokenData> RefreshTokenAsync(AccessTokenData accessTokenData, double refreshTokenExpiryInDays);
        public Task<IdentityResult> RegisterUserAsync(User user, string password);
    }
}