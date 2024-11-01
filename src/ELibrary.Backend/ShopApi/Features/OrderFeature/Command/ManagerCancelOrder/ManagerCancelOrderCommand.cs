using MediatR;

namespace ShopApi.Features.OrderFeature.Command.ManagerCancelOrder
{
    public record ManagerCancelOrderCommand(int OrderId) : IRequest<Unit>;
}
