using LibraryShopEntities.Domain.Entities.Shop;
using MediatR;
using ShopApi.Features.OrderFeature.Services;
using ShopApi.Features.StockBookOrderFeature.Services;

namespace ShopApi.Features.OrderFeature.Command.ManagerCancelOrder
{
    public class ManagerCancelOrderCommandHandler : IRequestHandler<ManagerCancelOrderCommand, Unit>
    {
        private readonly IOrderService orderService;
        private readonly IStockBookOrderService stockBookOrderService;

        public ManagerCancelOrderCommandHandler(
            IOrderService orderService,
            IStockBookOrderService stockBookOrderService)
        {
            this.orderService = orderService;
            this.stockBookOrderService = stockBookOrderService;
        }

        public async Task<Unit> Handle(ManagerCancelOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await orderService.GetOrderByIdAsync(request.OrderId, cancellationToken);

            if (order == null)
            {
                throw new InvalidOperationException("Order is not found.");
            }

            order.OrderStatus = OrderStatus.Canceled;
            var canceledOrder = await orderService.UpdateOrderAsync(order, cancellationToken);

            await stockBookOrderService.AddStockBookOrderAsyncFromCanceledOrderAsync(canceledOrder, StockBookOrderType.ManagerOrderCancel, cancellationToken);

            return Unit.Value;
        }
    }
}
