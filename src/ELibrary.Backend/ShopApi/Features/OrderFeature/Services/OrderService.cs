using LibraryShopEntities.Domain.Entities.Shop;
using LibraryShopEntities.Filters;
using LibraryShopEntities.Repositories.Shop;
using Microsoft.EntityFrameworkCore;

namespace ShopApi.Features.OrderFeature.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository repository;

        public OrderService(IOrderRepository repository)
        {
            this.repository = repository;
        }

        #region IOrderService Members

        public async Task<Order?> GetOrderByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await repository.GetOrderByIdAsync(id, cancellationToken);
        }
        public async Task<int> GetOrderAmountAsync(GetOrdersFilter filter, CancellationToken cancellationToken)
        {
            return await repository.GetOrderCountAsync(filter, cancellationToken);
        }
        public async Task<IEnumerable<Order>> GetPaginatedOrdersAsync(GetOrdersFilter filter, CancellationToken cancellationToken)
        {
            return await repository.GetPaginatedOrdersAsync(filter, cancellationToken);
        }
        public async Task<Order> CreateOrderAsync(Order order, CancellationToken cancellationToken)
        {
            order.OrderAmount = order.OrderBooks.Sum(x => x.BookAmount);

            order.TotalPrice = order.OrderBooks
                .Sum(orderBook => orderBook.BookAmount * orderBook.BookPrice);

            var newOrder = await repository.AddOrderAsync(order, cancellationToken);

            return await repository.GetOrderByIdAsync(newOrder.Id, cancellationToken)
                ?? throw new InvalidOperationException("Failed to retrieve the created order.");
        }
        public async Task<Order> UpdateOrderAsync(Order order, CancellationToken cancellationToken)
        {
            var orderInDb = await repository.GetOrderByIdAsync(order.Id, cancellationToken);

            if (orderInDb == null)
            {
                throw new InvalidOperationException("Order is not found.");
            }

            if (orderInDb.OrderStatus != OrderStatus.InProcessing)
            {
                throw new InvalidOperationException("Only orders that are in processing can be updated.");
            }

            orderInDb.Copy(order);
            await repository.UpdateOrderAsync(orderInDb, cancellationToken);

            return await repository.GetOrderByIdAsync(orderInDb.Id, cancellationToken)
                ?? throw new InvalidOperationException("Failed to retrieve the updated order.");
        }
        public async Task DeleteOrderAsync(int id, CancellationToken cancellationToken)
        {
            var order = await repository.GetOrderByIdAsync(id, cancellationToken);
            if (order != null)
            {
                await repository.DeleteOrderAsync(order, cancellationToken);
            }
        }

        #endregion
    }
}