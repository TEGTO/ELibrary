using LibraryShopEntities.Filters;
using MediatR;

namespace ShopApi.Features.OrderFeature.Command.GetOrderAmount
{
    public record GetOrderAmountQuery(string UserId, GetOrdersFilter Request) : IRequest<int>;
}
