using Microsoft.EntityFrameworkCore;
using Shared.Repositories;
using UserEntities.Data;
using UserEntities.Domain.Entities;

namespace UserApi.Services
{
    public class UserAuthenticationMethodService : IUserAuthenticationMethodService
    {
        private readonly IDatabaseRepository<UserIdentityDbContext> databaseRepository;

        public UserAuthenticationMethodService(IDatabaseRepository<UserIdentityDbContext> databaseRepository)
        {
            this.databaseRepository = databaseRepository;
        }

        public async Task<List<AuthenticationMethod>> GetUserAuthenticationMethodsAsync(User user, CancellationToken cancellationToken)
        {
            var queryable = await databaseRepository.GetQueryableAsync<UserAuthenticationMethod>(cancellationToken);
            return await queryable.Where(x => x.UserId == user.Id).Select(x => x.AuthenticationMethod).ToListAsync();
        }
        public async Task SetUserAuthenticationMethodAsync(User user, AuthenticationMethod method, CancellationToken cancellationToken)
        {
            var queryable = await databaseRepository.GetQueryableAsync<User>(cancellationToken);

            var userInDb = await queryable.FirstAsync(x => x.Id == user.Id);

            if (userInDb.AuthenticationMethods.Find(x => x.AuthenticationMethod == method) == null)
            {
                userInDb.AuthenticationMethods.Add(
                   new UserAuthenticationMethod()
                   {
                       User = userInDb,
                       UserId = user.Id,
                       AuthenticationMethod = method
                   }
                );
                await databaseRepository.UpdateAsync(userInDb, cancellationToken);
            }
        }
    }
}
