using LibraryShopEntities.Domain.Entities.Shop;

namespace LibraryShopEntities.Repositories.Shop
{
    public interface IClientRepository
    {
        public Task<Client> CreateClientAsync(Client client, CancellationToken cancellationToken);
        public Task DeleteClientAsync(Client client, CancellationToken cancellationToken);
        public Task<Client?> GetClientByUserIdAsync(string userId, CancellationToken cancellationToken);
        public Task<Client> UpdateClientAsync(Client client, CancellationToken cancellationToken);
    }
}