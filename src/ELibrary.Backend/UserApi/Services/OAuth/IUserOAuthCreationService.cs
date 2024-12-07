using UserApi.Domain.Models;
using UserEntities.Domain.Entities;

namespace UserApi.Services.OAuth
{
    public interface IUserOAuthCreationService
    {
        public Task<User?> CreateUserFromOAuthAsync(CreateUserFromOAuthModel model, CancellationToken cancellationToken);
    }
}