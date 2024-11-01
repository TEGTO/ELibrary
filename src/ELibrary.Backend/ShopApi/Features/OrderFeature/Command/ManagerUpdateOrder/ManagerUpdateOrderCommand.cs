using LibraryShopEntities.Domain.Dtos.Shop;
using MediatR;
using ShopApi.Features.OrderFeature.Dtos;

namespace ShopApi.Features.OrderFeature.Command.ManagerUpdateOrder
{
    public record ManagerUpdateOrderCommand(ManagerUpdateOrderRequest Request) : IRequest<OrderResponse>;
}
