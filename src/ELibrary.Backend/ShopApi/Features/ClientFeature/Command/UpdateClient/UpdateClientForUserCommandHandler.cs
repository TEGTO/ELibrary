using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using MediatR;
using ShopApi.Features.ClientFeature.Services;

namespace ShopApi.Features.ClientFeature.Command.UpdateClient
{
    public class UpdateClientForUserCommandHandler : IRequestHandler<UpdateClientForUserCommand, ClientResponse>
    {
        private readonly IClientService clientService;
        private readonly IMapper mapper;

        public UpdateClientForUserCommandHandler(IClientService clientService, IMapper mapper)
        {
            this.clientService = clientService;
            this.mapper = mapper;
        }

        public async Task<ClientResponse> Handle(UpdateClientForUserCommand command, CancellationToken cancellationToken)
        {
            var client = mapper.Map<Client>(command.Request);

            client.UserId = command.UserId;

            var updatedClient = await clientService.UpdateClientAsync(client, cancellationToken);
            return mapper.Map<ClientResponse>(updatedClient);
        }
    }
}
