using Authentication.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using UserApi.Domain.Models;
using UserEntities.Domain.Entities;

namespace UserApi.Services
{
    public record class RegisterUserParams(User User, string Password);
    public record class LoginUserParams(string Login, string Password, double RefreshTokenExpiryInDays);
    public interface IAuthService
    {
        public Task<AccessTokenData> LoginUserAsync(LoginUserParams loginParams);
        public Task<User?> GetUserAsync(ClaimsPrincipal principal);
        public Task<User?> GetUserByLoginAsync(string login);
        public Task<List<IdentityError>> SetUserRolesAsync(User user, List<string> roles);
        public Task<List<string>> GetUserRolesAsync(User user);
        public Task<AccessTokenData> RefreshTokenAsync(AccessTokenData accessTokenData, double refreshTokenExpiryInDays);
        public Task<List<IdentityError>> UpdateUserAsync(User user, UserUpdateData updateData, bool resetPassword);
        public Task<IdentityResult> RegisterUserAsync(RegisterUserParams registerParams);
        public Task<IdentityResult> DeleteUserAsync(User user);
    }
}