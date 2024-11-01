using LibraryShopEntities.Domain.Dtos.Shop;
using MediatR;
using ShopApi.Features.OrderFeature.Dtos;

namespace ShopApi.Features.OrderFeature.Command.ManagerGetPaginatedOrders
{
    public record ManagerGetPaginatedOrdersQuery(GetOrdersFilter Filter) : IRequest<IEnumerable<OrderResponse>>;
}
