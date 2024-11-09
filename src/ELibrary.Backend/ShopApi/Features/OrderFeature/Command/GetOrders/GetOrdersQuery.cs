using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Filters;
using MediatR;

namespace ShopApi.Features.OrderFeature.Command.GetOrders
{
    public record GetOrdersQuery(string UserId, GetOrdersFilter Request) : IRequest<IEnumerable<OrderResponse>>;
}
