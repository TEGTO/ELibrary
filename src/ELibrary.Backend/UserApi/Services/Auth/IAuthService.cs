using Authentication.Models;
using Microsoft.AspNetCore.Identity;
using UserApi.Domain.Models;

namespace UserApi.Services.Auth
{
    public interface IAuthService
    {
        public Task<AccessTokenData> LoginUserAsync(LoginUserModel loginModel, CancellationToken cancellationToken);
        public Task<AccessTokenData> RefreshTokenAsync(RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken);
        public Task<IdentityResult> RegisterUserAsync(RegisterUserModel registerModel, CancellationToken cancellationToken);
    }
}