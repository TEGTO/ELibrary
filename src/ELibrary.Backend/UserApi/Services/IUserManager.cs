using System.Security.Claims;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Requests;
using UserApi.Domain.Dtos.Responses;

namespace UserApi.Services
{
    public interface IUserManager
    {
        public Task<UserAuthenticationResponse> LoginUserAsync(UserAuthenticationRequest request);
        public Task<AuthToken> RefreshTokenAsync(AuthToken request);
        public Task<UserAuthenticationResponse> RegisterUserAsync(UserRegistrationRequest request);
        public Task DeleteUserAsync(ClaimsPrincipal user, CancellationToken cancellationToken);
        public Task<AdminUserResponse> GetUserByInfoAsync(string info);
        public Task<IEnumerable<AdminUserResponse>> GetPaginatedUsersAsync(AdminGetUserFilter filter, CancellationToken cancellationToken);
        public Task<int> GetPaginatedUserTotalAmountAsync(AdminGetUserFilter filter, CancellationToken cancellationToken);
        public Task UpdateUserAsync(UserUpdateDataRequest request, ClaimsPrincipal userClaims, CancellationToken cancellationToken);
        public Task AdminDeleteUserAsync(string login);
        public Task<AdminUserResponse> AdminRegisterUserAsync(AdminUserRegistrationRequest request);
        public Task<AdminUserResponse> AdminUpdateUserAsync(AdminUserUpdateDataRequest request, CancellationToken cancellationToken);
    }
}