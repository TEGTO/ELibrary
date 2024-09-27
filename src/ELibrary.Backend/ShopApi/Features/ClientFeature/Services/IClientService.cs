using LibraryShopEntities.Domain.Entities.Shop;

namespace ShopApi.Features.ClientFeature.Services
{
    public interface IClientService
    {
        public Task<Client> CreateClientAsync(Client client, CancellationToken cancellationToken);
        public Task DeleteClientAsync(string id, CancellationToken cancellationToken);
        public Task<Client?> GetClientByUserIdAsync(string userId, CancellationToken cancellationToken);
        public Task<Client> UpdateClientAsync(Client client, CancellationToken cancellationToken);
    }
}