using LibraryShopEntities.Domain.Entities.Shop;
using LibraryShopEntities.Repositories.Shop;

namespace ShopApi.Features.ClientFeature.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository repository;

        public ClientService(IClientRepository repository)
        {
            this.repository = repository;
        }

        #region IClientService Members

        public async Task<Client?> GetClientByUserIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await repository.GetClientByUserIdAsync(userId, cancellationToken);
        }
        public async Task<Client> CreateClientAsync(Client client, CancellationToken cancellationToken)
        {
            return await repository.CreateClientAsync(client, cancellationToken);
        }
        public async Task<Client> UpdateClientAsync(Client client, CancellationToken cancellationToken)
        {
            var clientInDb = await repository.GetClientByUserIdAsync(client.UserId, cancellationToken);

            if (clientInDb == null)
            {
                throw new InvalidOperationException("Update failed. Client is null!");
            }

            clientInDb.Copy(client);
            return await repository.UpdateClientAsync(clientInDb, cancellationToken);
        }
        public async Task DeleteClientAsync(string userId, CancellationToken cancellationToken)
        {
            var client = await repository.GetClientByUserIdAsync(userId, cancellationToken);

            if (client != null)
            {
                await repository.DeleteClientAsync(client, cancellationToken);
            }
        }

        #endregion
    }
}
