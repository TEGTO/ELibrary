using MediatR;
using ShopApi.Features.OrderFeature.Dtos;

namespace ShopApi.Features.OrderFeature.Command.ManagerGetOrderAmount
{
    public record ManagerGetOrderAmountQuery(GetOrdersFilter Filter) : IRequest<int>;
}
