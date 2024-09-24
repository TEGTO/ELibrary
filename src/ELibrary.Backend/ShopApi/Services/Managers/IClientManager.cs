using LibraryShopEntities.Domain.Dtos.Shop;
using ShopApi.Domain.Dtos.Client;

namespace ShopApi.Services.Facades
{
    public interface IClientManager
    {
        public Task<ClientResponse?> GetClientForUserAsync(string userId, CancellationToken cancellationToken);
        public Task<ClientResponse> CreateClientForUserAsync(string userId, CreateClientRequest request, CancellationToken cancellationToken);
        public Task DeleteClientForUserAsync(string userId, CancellationToken cancellationToken);
        public Task<ClientResponse> UpdateClientForUserAsync(string userId, UpdateClientRequest request, CancellationToken cancellationToken);
    }
}