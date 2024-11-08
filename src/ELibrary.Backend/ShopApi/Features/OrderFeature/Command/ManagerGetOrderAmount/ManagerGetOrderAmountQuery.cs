using LibraryShopEntities.Filters;
using MediatR;

namespace ShopApi.Features.OrderFeature.Command.ManagerGetOrderAmount
{
    public record ManagerGetOrderAmountQuery(GetOrdersFilter Filter) : IRequest<int>;
}
