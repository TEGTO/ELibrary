using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using ShopApi.Features.ClientFeature.Dtos;

namespace ShopApi.Features.ClientFeature.Services
{
    public class ClientManager : IClientManager
    {
        private readonly IClientService clientService;
        private readonly IMapper mapper;

        public ClientManager(IClientService clientService, IMapper mapper)
        {
            this.clientService = clientService;
            this.mapper = mapper;
        }

        public async Task<ClientResponse?> GetClientForUserAsync(string userId, CancellationToken cancellationToken)
        {
            var client = await clientService.GetClientByUserIdAsync(userId, cancellationToken);
            return mapper.Map<ClientResponse>(client);
        }
        public async Task<ClientResponse> CreateClientForUserAsync(string userId, CreateClientRequest request, CancellationToken cancellationToken)
        {
            var client = mapper.Map<Client>(request);
            client.UserId = userId;
            var createdClient = await clientService.CreateClientAsync(client, cancellationToken);
            return mapper.Map<ClientResponse>(createdClient);
        }
        public async Task<ClientResponse> UpdateClientForUserAsync(string userId, UpdateClientRequest request, CancellationToken cancellationToken)
        {
            var client = await clientService.GetClientByUserIdAsync(userId, cancellationToken);

            if (client == null)
            {
                throw new InvalidOperationException("Client doesn't exist!");
            }

            client.Copy(mapper.Map<Client>(request));
            var updatedClient = await clientService.UpdateClientAsync(client, cancellationToken);
            return mapper.Map<ClientResponse>(updatedClient);
        }
        public async Task DeleteClientForUserAsync(string userId, CancellationToken cancellationToken)
        {
            var client = await clientService.GetClientByUserIdAsync(userId, cancellationToken);

            if (client == null)
            {
                throw new InvalidOperationException("Client doesn't exist!");
            }

            await clientService.DeleteClientAsync(client.Id, cancellationToken);
        }
    }
}