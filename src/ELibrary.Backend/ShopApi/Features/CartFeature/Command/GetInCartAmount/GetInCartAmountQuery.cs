using MediatR;

namespace ShopApi.Features.CartFeature.Command.GetInCartAmount
{
    public record GetInCartAmountQuery(string UserId) : IRequest<int>;
}
