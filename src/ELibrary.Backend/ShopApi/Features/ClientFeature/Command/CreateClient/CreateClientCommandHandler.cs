using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using MediatR;
using ShopApi.Features.ClientFeature.Services;

namespace ShopApi.Features.ClientFeature.Command.CreateClient
{
    public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, ClientResponse>
    {
        private readonly IClientService clientService;
        private readonly IMapper mapper;

        public CreateClientCommandHandler(IClientService clientService, IMapper mapper)
        {
            this.clientService = clientService;
            this.mapper = mapper;
        }

        public async Task<ClientResponse> Handle(CreateClientCommand command, CancellationToken cancellationToken)
        {
            var client = mapper.Map<Client>(command.Request);
            client.UserId = command.UserId;
            var createdClient = await clientService.CreateClientAsync(client, cancellationToken);
            return mapper.Map<ClientResponse>(createdClient);
        }
    }
}
