using LibraryShopEntities.Domain.Entities.Shop;
using LibraryShopEntities.Filters;

namespace LibraryShopEntities.Repositories.Shop
{
    public interface IOrderRepository
    {
        public Task<Order> AddOrderAsync(Order order, CancellationToken cancellationToken);
        public Task DeleteOrderAsync(Order order, CancellationToken cancellationToken);
        public Task<Order?> GetOrderByIdAsync(int id, CancellationToken cancellationToken);
        public Task<int> GetOrderCountAsync(GetOrdersFilter filter, CancellationToken cancellationToken);
        public Task<IEnumerable<Order>> GetPaginatedOrdersAsync(GetOrdersFilter filter, CancellationToken cancellationToken);
        public Task<Order> UpdateOrderAsync(Order order, CancellationToken cancellationToken);
    }
}