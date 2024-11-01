using MediatR;
using ShopApi.Features.OrderFeature.Dtos;

namespace ShopApi.Features.OrderFeature.Command.GetOrderAmount
{
    public record GetOrderAmountQuery(string UserId, GetOrdersFilter Request) : IRequest<int>;
}
