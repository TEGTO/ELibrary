using LibraryShopEntities.Domain.Dtos.Shop;
using MediatR;
using ShopApi.Features.OrderFeature.Dtos;

namespace ShopApi.Features.OrderFeature.Command.UpdateOrder
{
    public record UpdateOrderCommand(string UserId, ClientUpdateOrderRequest Request) : IRequest<OrderResponse>;
}
