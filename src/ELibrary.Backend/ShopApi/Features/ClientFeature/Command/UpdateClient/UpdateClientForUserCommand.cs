using LibraryShopEntities.Domain.Dtos.Shop;
using MediatR;
using ShopApi.Features.ClientFeature.Dtos;

namespace ShopApi.Features.ClientFeature.Command.UpdateClient
{
    public record UpdateClientForUserCommand(string UserId, UpdateClientRequest Request) : IRequest<ClientResponse>;
}
