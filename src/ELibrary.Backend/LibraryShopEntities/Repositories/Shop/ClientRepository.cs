using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Shop;
using Microsoft.EntityFrameworkCore;
using Shared.Repositories;

namespace LibraryShopEntities.Repositories.Shop
{
    public class ClientRepository : IClientRepository
    {
        private readonly IDatabaseRepository<ShopDbContext> repository;

        public ClientRepository(IDatabaseRepository<ShopDbContext> repository)
        {
            this.repository = repository;
        }

        #region IClientService Members

        public async Task<Client?> GetClientByUserIdAsync(string userId, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Client>(cancellationToken);
            return await queryable.AsNoTracking().FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);
        }
        public async Task<Client> CreateClientAsync(Client client, CancellationToken cancellationToken)
        {
            return await repository.AddAsync(client, cancellationToken);
        }
        public async Task<Client> UpdateClientAsync(Client client, CancellationToken cancellationToken)
        {
            return await repository.UpdateAsync(client, cancellationToken);
        }
        public async Task DeleteClientAsync(Client client, CancellationToken cancellationToken)
        {
            await repository.DeleteAsync(client, cancellationToken);
        }

        #endregion
    }
}
