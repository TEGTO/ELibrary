using Authentication.Models;
using Microsoft.AspNetCore.Identity;
using UserEntities.Domain.Entities;

namespace UserApi.Services.Auth
{
    public record class RegisterUserParams(User User, string Password);
    public record class LoginUserParams(User User, string Password);
    public record class RefreshTokenParams(User User, AccessTokenData AccessTokenData);
    public interface IAuthService
    {
        public Task<AccessTokenData> LoginUserAsync(LoginUserParams loginParams, CancellationToken cancellationToken);
        public Task<AccessTokenData> RefreshTokenAsync(RefreshTokenParams refreshTokenParams, CancellationToken cancellationToken);
        public Task<IdentityResult> RegisterUserAsync(RegisterUserParams registerParams, CancellationToken cancellationToken);
    }
}