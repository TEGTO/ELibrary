using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using Shared.Domain.Dtos;
using ShopApi.Features.OrderFeature.Dtos;
using ShopApi.Features.StockBookOrderFeature.Services;

namespace ShopApi.Features.OrderFeature.Services
{
    public class OrderManager : IOrderManager
    {
        private readonly IMapper mapper;
        private readonly IOrderService orderService;
        private readonly IStockBookOrderService stockBookOrderService;

        public OrderManager(IMapper mapper, IOrderService orderService, IStockBookOrderService stockBookOrderService)
        {
            this.mapper = mapper;
            this.orderService = orderService;
            this.stockBookOrderService = stockBookOrderService;
        }

        #region Client

        public async Task<IEnumerable<OrderResponse>> GetPaginatedOrdersAsync(string clientId, PaginationRequest pagination, CancellationToken cancellationToken)
        {
            var orders = await orderService.GetPaginatedOrdersAsync(clientId, pagination, cancellationToken);
            return orders.Select(mapper.Map<OrderResponse>);
        }
        public async Task<int> GetOrderAmountAsync(string clientId, CancellationToken cancellationToken)
        {
            return await orderService.GetOrderAmountAsync(clientId, cancellationToken);
        }
        public async Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request, Client client, CancellationToken cancellationToken)
        {
            var order = mapper.Map<Order>(request);
            order.ClientId = client.Id;

            await stockBookOrderService.AddStockBookOrderAsyncFromOrderAsync(order, StockBookOrderType.ClientOrder, cancellationToken);
            var createdOrder = await orderService.CreateOrderAsync(order, cancellationToken);

            return mapper.Map<OrderResponse>(createdOrder);
        }
        public async Task<OrderResponse> UpdateOrderAsync(ClientUpdateOrderRequest request, Client client, CancellationToken cancellationToken)
        {
            var order = await orderService.GetOrderByIdAsync(request.Id, cancellationToken);

            if (order == null || order.ClientId != client.Id)
            {
                throw new InvalidOperationException("Order not found.");
            }

            mapper.Map(request, order);
            var updatedOrder = await orderService.UpdateOrderAsync(order, cancellationToken);

            return mapper.Map<OrderResponse>(updatedOrder);
        }
        public async Task<OrderResponse> CancelOrderAsync(int orderId, Client client, CancellationToken cancellationToken)
        {
            var order = await orderService.GetOrderByIdAsync(orderId, cancellationToken);

            if (order == null || order.ClientId != client.Id)
            {
                throw new InvalidOperationException("Order not found.");
            }

            if (order.OrderStatus != OrderStatus.InProcessing)
            {
                throw new InvalidOperationException("It is not possible to client to cancel an order with this order status.");
            }

            order.OrderStatus = OrderStatus.Canceled;
            var canceledOrder = await orderService.UpdateOrderAsync(order, cancellationToken);

            await stockBookOrderService.AddStockBookOrderAsyncFromCanceledOrderAsync(canceledOrder, StockBookOrderType.ClientOrderCancel, cancellationToken);

            return mapper.Map<OrderResponse>(canceledOrder);
        }

        #endregion

        #region Manager

        public async Task<OrderResponse?> GetOrderByIdAsync(int id, CancellationToken cancellationToken)
        {
            var order = await orderService.GetOrderByIdAsync(id, cancellationToken);
            return mapper.Map<OrderResponse>(order);
        }
        public async Task<IEnumerable<OrderResponse>> GetPaginatedOrdersAsync(PaginationRequest request, CancellationToken cancellationToken)
        {
            var orders = await orderService.GetPaginatedOrdersAsync(request, cancellationToken);
            return orders.Select(mapper.Map<OrderResponse>);
        }
        public async Task<int> GetOrderAmountAsync(CancellationToken cancellationToken)
        {
            return await orderService.GetOrderAmountAsync(cancellationToken);
        }
        public async Task<OrderResponse> UpdateOrderAsync(ManagerUpdateOrderRequest request, CancellationToken cancellationToken)
        {
            var order = mapper.Map<Order>(request);
            var updatedOrder = await orderService.UpdateOrderAsync(order, cancellationToken);
            return mapper.Map<OrderResponse>(updatedOrder);
        }
        public async Task<OrderResponse> CancelOrderAsync(int orderId, CancellationToken cancellationToken)
        {
            var order = await orderService.GetOrderByIdAsync(orderId, cancellationToken);

            if (order == null)
            {
                throw new InvalidOperationException("Order not found.");
            }

            order.OrderStatus = OrderStatus.Canceled;
            var canceledOrder = await orderService.UpdateOrderAsync(order, cancellationToken);

            await stockBookOrderService.AddStockBookOrderAsyncFromCanceledOrderAsync(canceledOrder, StockBookOrderType.ManagerOrderCancel, cancellationToken);

            return mapper.Map<OrderResponse>(canceledOrder);
        }

        #endregion
    }
}
