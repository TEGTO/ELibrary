using LibraryShopEntities.Domain.Dtos.Shop;
using MediatR;
using ShopApi.Features.OrderFeature.Dtos;

namespace ShopApi.Features.OrderFeature.Command.GetOrders
{
    public record GetOrdersQuery(string UserId, GetOrdersFilter Request) : IRequest<IEnumerable<OrderResponse>>;
}
