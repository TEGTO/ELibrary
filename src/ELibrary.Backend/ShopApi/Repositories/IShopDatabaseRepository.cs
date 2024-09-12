using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Shop;
using Shared.Repositories;

namespace ShopApi.Repositories
{
    public interface IShopDatabaseRepository : IDatabaseRepository<LibraryShopDbContext>
    {
        public Task<Order> CreateOrderAsync(Order order, List<int> bookIds, CancellationToken cancellationToken);
        public Task<Order> UpdateOrderAsync(Order order, List<int> bookIds, CancellationToken cancellationToken);
    }
}