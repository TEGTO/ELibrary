using LibraryShopEntities.Domain.Entities.Shop;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using ShopApi.Repositories;

namespace ShopApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly IShopDatabaseRepository repository;

        public OrderService(IShopDatabaseRepository repository)
        {
            this.repository = repository;
        }

        #region IOrderService Members

        public async Task<Order?> GetOrderByIdAsync(int id, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Order>(cancellationToken);
            return await queryable.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
        public async Task<IEnumerable<Order>> GetPaginatedAsync(PaginationRequest pagination, CancellationToken cancellationToken)
        {
            List<Order> orders = new List<Order>();
            var queryable = await repository.GetQueryableAsync<Order>(cancellationToken);

            orders.AddRange(await queryable
                                     .AsNoTracking()
                                     .OrderByDescending(b => b.Id)
                                     .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                                     .Take(pagination.PageSize)
                                     .Include(x => x.Books)
                                     .ToListAsync(cancellationToken));
            return orders;
        }
        public async Task<IEnumerable<Order>> GetOrdersByClientIdAsync(string id, CancellationToken cancellationToken)
        {
            List<Order> orders = new List<Order>();
            var queryable = await repository.GetQueryableAsync<Order>(cancellationToken);

            orders.AddRange(
            await queryable.AsNoTracking()
            .Where(t => t.ClientId == id)
            .Include(x => x.Books)
            .ToListAsync(cancellationToken));

            return orders;
        }
        public async Task<Order> CreateOrderAsync(Order order, CancellationToken cancellationToken)
        {
            var bookIds = order.Books.Select(b => b.Id).ToList();
            return await repository.CreateOrderAsync(order, bookIds, cancellationToken);
        }
        public async Task<bool> CheckOrderAsync(string clientId, int id, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Order>(cancellationToken);
            return await queryable.AsNoTracking().AnyAsync(x => x.Id == id && x.ClientId == clientId, cancellationToken);
        }
        public async Task<Order> UpdateOrderAsync(Order order, CancellationToken cancellationToken)
        {
            var bookIds = new List<int>();

            if (order.Books.Count > 0)
            {
                bookIds.AddRange(order.Books.Select(b => b.Id));
            }

            return await repository.UpdateOrderAsync(order, bookIds, cancellationToken);

        }
        public async Task DeleteOrderAsync(int id, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Order>(cancellationToken);
            var entityInDb = await queryable.FirstAsync(x => x.Id == id, cancellationToken);
            await repository.DeleteAsync(entityInDb, cancellationToken);
        }

        #endregion
    }
}