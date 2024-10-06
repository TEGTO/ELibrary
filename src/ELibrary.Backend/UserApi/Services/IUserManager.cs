using System.Security.Claims;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Requests;
using UserApi.Domain.Dtos.Responses;

namespace UserApi.Services
{
    public interface IUserManager
    {
        public Task AdminDeleteUserAsync(string login);
        public Task<AdminUserResponse> AdminRegisterUserAsync(AdminUserRegistrationRequest request);
        public Task AdminUpdateUserAsync(AdminUserUpdateDataRequest request, CancellationToken cancellationToken);
        public Task<IEnumerable<AdminUserResponse>> GetPaginatedUsersAsync(GetUserFilterRequest filter, CancellationToken cancellationToken);
        public Task<int> GetPaginatedUserTotalAmountAsync(GetUserFilterRequest filter, CancellationToken cancellationToken);
        public Task<AdminUserResponse> GetUserByInfoAsync(string info);
        public Task<UserAuthenticationResponse> LoginUserAsync(UserAuthenticationRequest request);
        public Task<AuthToken> RefreshTokenAsync(AuthToken request);
        public Task<UserAuthenticationResponse> RegisterUserAsync(UserRegistrationRequest request);
        public Task UpdateUserAsync(UserUpdateDataRequest request, ClaimsPrincipal userClaims, CancellationToken cancellationToken);
    }
}