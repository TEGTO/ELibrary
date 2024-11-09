using LibraryShopEntities.Domain.Entities.Shop;
using MediatR;
using ShopApi.Features.ClientFeature.Services;
using ShopApi.Features.OrderFeature.Services;
using ShopApi.Features.StockBookOrderFeature.Services;

namespace ShopApi.Features.OrderFeature.Command.CancelOrder
{
    public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, Unit>
    {
        private readonly IOrderService orderService;
        private readonly IClientService clientService;
        private readonly IStockBookOrderService stockBookOrderService;

        public CancelOrderCommandHandler(
            IOrderService orderService,
            IClientService clientService,
            IStockBookOrderService stockBookOrderService)
        {
            this.orderService = orderService;
            this.clientService = clientService;
            this.stockBookOrderService = stockBookOrderService;
        }

        public async Task<Unit> Handle(CancelOrderCommand command, CancellationToken cancellationToken)
        {
            var client = await clientService.GetClientByUserIdAsync(command.UserId, cancellationToken);

            if (client == null)
            {
                throw new InvalidDataException("Client is not found!");
            }

            var order = await orderService.GetOrderByIdAsync(command.OrderId, cancellationToken);

            if (order == null || order.ClientId != client.Id)
            {
                throw new InvalidOperationException("Order is not found.");
            }

            order.OrderStatus = OrderStatus.Canceled;

            var canceledOrder = await orderService.UpdateOrderAsync(order, cancellationToken);

            await stockBookOrderService.AddStockBookOrderAsyncFromCanceledOrderAsync(canceledOrder, StockBookOrderType.ClientOrderCancel, cancellationToken);

            return Unit.Value;
        }
    }
}
