using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Domain.Entities.Shop;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using Shared.Repositories;

namespace ShopApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly IDatabaseRepository<LibraryShopDbContext> repository;

        public OrderService(IDatabaseRepository<LibraryShopDbContext> repository)
        {
            this.repository = repository;
        }

        #region IOrderService Members

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
            var dbContext = await repository.CreateDbContextAsync(cancellationToken);

            var queryable = dbContext.Set<Book>().AsQueryable();

            var bookIds = order.Books.Select(b => b.Id).ToList();
            var booksFromDb = await queryable
                .Where(b => bookIds.Contains(b.Id))
                .ToListAsync(cancellationToken);

            order.Books = booksFromDb;

            await dbContext.AddAsync(order, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return order;
        }
        public async Task<bool> CheckOrderAsync(string clientId, int id, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Order>(cancellationToken);
            return await queryable.AsNoTracking().AnyAsync(x => x.Id == id && x.ClientId == clientId, cancellationToken);
        }
        public async Task<Order> UpdateOrderAsync(Order order, CancellationToken cancellationToken)
        {
            var dbContext = await repository.CreateDbContextAsync(cancellationToken);

            var queryableOrders = dbContext.Set<Order>();
            var orderInDb = await queryableOrders.FirstAsync(x => x.Id == order.Id, cancellationToken);

            orderInDb.Copy(order);

            if (orderInDb.OrderStatus == OrderStatus.InProcessing)
            {
                var queryable = dbContext.Set<Book>().AsQueryable();

                var bookIds = order.Books.Select(b => b.Id).ToList();
                var booksFromDb = await queryable
                    .Where(b => bookIds.Contains(b.Id))
                    .ToListAsync(cancellationToken);

                order.Books = booksFromDb;
            }

            dbContext.Update(orderInDb);
            await dbContext.SaveChangesAsync(cancellationToken);
            return orderInDb;
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