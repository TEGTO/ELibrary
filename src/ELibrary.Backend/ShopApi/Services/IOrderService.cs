using LibraryShopEntities.Domain.Entities.Shop;
using Shared.Dtos;

namespace ShopApi.Services
{
    public interface IOrderService
    {
        public Task<Order?> GetOrderByIdAsync(int id, CancellationToken cancellationToken);
        public Task<Order> CreateOrderAsync(Order order, CancellationToken cancellationToken);
        public Task DeleteOrderAsync(int id, CancellationToken cancellationToken);
        public Task<IEnumerable<Order>> GetPaginatedOrdersAsync(string clientId, PaginationRequest pagination, CancellationToken cancellationToken);
        public Task<IEnumerable<Order>> GetPaginatedOrdersAsync(PaginationRequest pagination, CancellationToken cancellationToken);
        public Task<int> GetOrderAmountAsync(string clientId, CancellationToken cancellationToken);
        public Task<int> GetOrderAmountAsync(CancellationToken cancellationToken);
        public Task<Order> UpdateOrderAsync(Order order, CancellationToken cancellationToken);
    }
}