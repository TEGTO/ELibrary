using System.Security.Claims;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Requests;
using UserApi.Domain.Dtos.Responses;

namespace UserApi.Services
{
    public interface IUserManager
    {
        public Task<UserAuthenticationResponse> LoginUserAsync(UserAuthenticationRequest request, CancellationToken cancellationToken);
        public Task<AuthToken> RefreshTokenAsync(AuthToken request, CancellationToken cancellationToken);
        public Task<UserAuthenticationResponse> RegisterUserAsync(UserRegistrationRequest request, CancellationToken cancellationToken);
        public Task DeleteUserAsync(ClaimsPrincipal user, CancellationToken cancellationToken);
        public Task<AdminUserResponse> GetUserByInfoAsync(string info, CancellationToken cancellationToken);
        public Task<IEnumerable<AdminUserResponse>> GetPaginatedUsersAsync(AdminGetUserFilter filter, CancellationToken cancellationToken);
        public Task<int> GetPaginatedUserTotalAmountAsync(AdminGetUserFilter filter, CancellationToken cancellationToken);
        public Task UpdateUserAsync(UserUpdateDataRequest request, ClaimsPrincipal userClaims, CancellationToken cancellationToken);
        public Task AdminDeleteUserAsync(string login, CancellationToken cancellationToken);
        public Task<AdminUserResponse> AdminRegisterUserAsync(AdminUserRegistrationRequest request, CancellationToken cancellationToken);
        public Task<AdminUserResponse> AdminUpdateUserAsync(AdminUserUpdateDataRequest request, CancellationToken cancellationToken);
    }
}