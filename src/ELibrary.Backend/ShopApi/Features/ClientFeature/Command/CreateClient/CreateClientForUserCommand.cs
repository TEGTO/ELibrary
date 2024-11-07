using LibraryShopEntities.Domain.Dtos.Shop;
using MediatR;
using ShopApi.Features.ClientFeature.Dtos;

namespace ShopApi.Features.ClientFeature.Command.CreateClient
{
    public record CreateClientForUserCommand(string UserId, CreateClientRequest Request) : IRequest<ClientResponse>;
}
