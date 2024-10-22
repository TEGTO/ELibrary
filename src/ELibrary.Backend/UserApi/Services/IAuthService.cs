using Authentication.Models;
using Microsoft.AspNetCore.Identity;
using UserEntities.Domain.Entities;

namespace UserApi.Services
{
    public record class RegisterUserParams(User User, string Password);
    public record class LoginUserParams(string Login, string Password, double RefreshTokenExpiryInDays);
    public interface IAuthService
    {
        public Task<AccessTokenData> LoginUserAsync(LoginUserParams loginParams);
        public Task<AccessTokenData> RefreshTokenAsync(AccessTokenData accessTokenData, double refreshTokenExpiryInDays);
        public Task<IdentityResult> RegisterUserAsync(RegisterUserParams registerParams);
    }
}