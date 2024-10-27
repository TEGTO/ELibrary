using Authentication.Models;
using Microsoft.AspNetCore.Identity;
using UserEntities.Domain.Entities;

namespace UserApi.Services.Auth
{
    public record class RegisterUserParams(User User, string Password);
    public record class LoginUserParams(string Login, string Password);
    public interface IAuthService
    {
        public Task<AccessTokenData> LoginUserAsync(LoginUserParams loginParams, CancellationToken cancellationToken);
        public Task<AccessTokenData> RefreshTokenAsync(AccessTokenData accessTokenData, CancellationToken cancellationToken);
        public Task<IdentityResult> RegisterUserAsync(RegisterUserParams registerParams, CancellationToken cancellationToken);
    }
}