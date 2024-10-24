using UserEntities.Domain.Entities;

namespace UserApi.Services
{
    public interface IUserAuthenticationMethodService
    {
        public Task<List<AuthenticationMethod>> GetUserAuthenticationMethodsAsync(User user, CancellationToken cancellationToken);
        public Task SetUserAuthenticationMethodAsync(User user, AuthenticationMethod method, CancellationToken cancellationToken);
    }
}