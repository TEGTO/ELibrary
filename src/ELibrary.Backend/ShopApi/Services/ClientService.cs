using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Shop;
using Microsoft.EntityFrameworkCore;
using Shared.Repositories;

namespace ShopApi.Services
{
    public class ClientService : IClientService
    {
        private readonly IDatabaseRepository<LibraryShopDbContext> repository;

        public ClientService(IDatabaseRepository<LibraryShopDbContext> repository)
        {
            this.repository = repository;
        }

        #region IClientService Members

        public async Task<Client?> GetClientByUserIdAsync(string userId, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Client>(cancellationToken);

            var client = await queryable.AsNoTracking().FirstOrDefaultAsync(t =>
            t.UserId == userId,
            cancellationToken);

            return client;
        }
        public async Task<Client> CreateClientAsync(Client client, CancellationToken cancellationToken)
        {
            return await repository.AddAsync(client, cancellationToken);
        }
        public async Task<Client> UpdateClientAsync(Client client, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Client>(cancellationToken);
            var entityInDb = await queryable.FirstAsync(x => x.Id == client.Id, cancellationToken);
            entityInDb.Copy(client);
            return await repository.UpdateAsync(client, cancellationToken);
        }
        public async Task DeleteClientAsync(string id, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Client>(cancellationToken);
            var entityInDb = await queryable.FirstAsync(x => x.Id == id, cancellationToken);
            await repository.DeleteAsync(entityInDb, cancellationToken);
        }

        #endregion
    }
}
