using LibraryShopEntities.Domain.Entities.Shop;
using LibraryShopEntities.Filters;

namespace ShopApi.Features.OrderFeature.Services
{
    public interface IOrderService
    {
        public Task<Order?> GetOrderByIdAsync(int id, CancellationToken cancellationToken);
        public Task<Order> CreateOrderAsync(Order order, CancellationToken cancellationToken);
        public Task DeleteOrderAsync(int id, CancellationToken cancellationToken);
        public Task<IEnumerable<Order>> GetPaginatedOrdersAsync(GetOrdersFilter filter, CancellationToken cancellationToken);
        public Task<int> GetOrderAmountAsync(GetOrdersFilter filter, CancellationToken cancellationToken);
        public Task<Order> UpdateOrderAsync(Order order, CancellationToken cancellationToken);
    }
}