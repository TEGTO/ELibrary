using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Filters;
using MediatR;

namespace ShopApi.Features.OrderFeature.Command.ManagerGetPaginatedOrders
{
    public record ManagerGetPaginatedOrdersQuery(GetOrdersFilter Filter) : IRequest<IEnumerable<OrderResponse>>;
}
