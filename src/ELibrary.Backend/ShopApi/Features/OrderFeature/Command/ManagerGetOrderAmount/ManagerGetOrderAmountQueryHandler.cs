using MediatR;
using ShopApi.Features.OrderFeature.Services;

namespace ShopApi.Features.OrderFeature.Command.ManagerGetOrderAmount
{
    public class ManagerGetOrderAmountQueryHandler : IRequestHandler<ManagerGetOrderAmountQuery, int>
    {
        private readonly IOrderService orderService;

        public ManagerGetOrderAmountQueryHandler(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        public async Task<int> Handle(ManagerGetOrderAmountQuery request, CancellationToken cancellationToken)
        {
            return await orderService.GetOrderAmountAsync(request.Filter, cancellationToken);
        }
    }
}
