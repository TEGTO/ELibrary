using MediatR;

namespace ShopApi.Features.OrderFeature.Command.CancelOrder
{
    public record CancelOrderCommand(string UserId, int OrderId) : IRequest<Unit>;
}
