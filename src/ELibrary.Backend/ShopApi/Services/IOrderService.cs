using LibraryShopEntities.Domain.Entities.Shop;
using Shared.Dtos;

namespace ShopApi.Services
{
    public interface IOrderService
    {
        public Task<Order?> GetOrderByIdAsync(int id, CancellationToken cancellationToken);
        public Task<bool> CheckOrderAsync(string clientId, int id, CancellationToken cancellationToken);
        public Task<Order> CreateOrderAsync(Order order, CancellationToken cancellationToken);
        public Task DeleteOrderAsync(int id, CancellationToken cancellationToken);
        public Task<IEnumerable<Order>> GetOrdersByClientIdAsync(string id, CancellationToken cancellationToken);
        public Task<IEnumerable<Order>> GetPaginatedAsync(PaginationRequest pagination, CancellationToken cancellationToken);
        public Task<Order> UpdateOrderAsync(Order order, CancellationToken cancellationToken);
    }
}