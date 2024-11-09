using LibraryShopEntities.Domain.Dtos.Shop;
using MediatR;
using ShopApi.Features.OrderFeature.Dtos;

namespace ShopApi.Features.OrderFeature.Command.CreateOrder
{
    public record CreateOrderCommand(string UserId, CreateOrderRequest Request) : IRequest<OrderResponse>;
}
