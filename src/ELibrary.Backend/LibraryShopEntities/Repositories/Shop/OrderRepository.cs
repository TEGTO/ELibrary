using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Shop;
using LibraryShopEntities.Filters;
using Microsoft.EntityFrameworkCore;
using Shared.Repositories;

namespace LibraryShopEntities.Repositories.Shop
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDatabaseRepository<ShopDbContext> repository;

        public OrderRepository(IDatabaseRepository<ShopDbContext> repository)
        {
            this.repository = repository;
        }

        public async Task<Order?> GetOrderByIdAsync(int id, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Order>(cancellationToken);
            return await GetQueryableOrder(queryable).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
        public async Task<int> GetOrderCountAsync(GetOrdersFilter filter, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Order>(cancellationToken);
            return await ApplyFilter(GetQueryableOrder(queryable), filter).CountAsync(cancellationToken);
        }
        public async Task<IEnumerable<Order>> GetPaginatedOrdersAsync(GetOrdersFilter filter, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Order>(cancellationToken);

            queryable = ApplyFilter(GetQueryableOrder(queryable), filter);

            return await ApplyPagination(queryable, filter).ToListAsync(cancellationToken);
        }
        public async Task<Order> AddOrderAsync(Order order, CancellationToken cancellationToken)
        {
            return await repository.AddAsync(order, cancellationToken);
        }
        public async Task<Order> UpdateOrderAsync(Order order, CancellationToken cancellationToken)
        {
            return await repository.UpdateAsync(order, cancellationToken);
        }
        public async Task DeleteOrderAsync(Order order, CancellationToken cancellationToken)
        {
            await repository.DeleteAsync(order, cancellationToken);
        }

        private IQueryable<Order> GetQueryableOrder(IQueryable<Order> queryable)
        {
            return queryable
                .AsSplitQuery()
                .AsNoTracking()
                .Include(o => o.OrderBooks)
                .Include(o => o.Client);
        }
        private IQueryable<Order> ApplyFilter(IQueryable<Order> queryable, GetOrdersFilter filter)
        {
            if (filter.ClientId != null)
            {
                queryable = queryable.Where(o => o.ClientId == filter.ClientId);
            }
            return queryable;
        }
        private IQueryable<Order> ApplyPagination(IQueryable<Order> queryable, GetOrdersFilter filter)
        {
            return queryable
                .OrderByDescending(o => o.CreatedAt)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize);
        }
    }
}
