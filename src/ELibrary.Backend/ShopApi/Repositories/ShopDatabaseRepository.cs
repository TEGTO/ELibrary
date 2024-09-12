using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Domain.Entities.Shop;
using Microsoft.EntityFrameworkCore;
using Shared.Repositories;

namespace ShopApi.Repositories
{
    public class ShopDatabaseRepository : DatabaseRepository<LibraryShopDbContext>, IShopDatabaseRepository
    {
        public ShopDatabaseRepository(IDbContextFactory<LibraryShopDbContext> contextFactory) : base(contextFactory)
        {
        }

        public async Task<Order> CreateOrderAsync(Order order, List<int> bookIds, CancellationToken cancellationToken)
        {
            var dbContext = await CreateDbContextAsync(cancellationToken);

            var queryable = dbContext.Set<Book>().AsQueryable();

            var booksFromDb = await queryable
                .Where(b => bookIds.Contains(b.Id))
                .ToListAsync(cancellationToken);

            order.Books = booksFromDb;

            await dbContext.AddAsync(order, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return order;
        }
        public async Task<Order> UpdateOrderAsync(Order order, List<int> bookIds, CancellationToken cancellationToken)
        {
            var dbContext = await CreateDbContextAsync(cancellationToken);

            var existingOrder = await dbContext.Orders
                .Include(o => o.Books)
                .FirstOrDefaultAsync(o => o.Id == order.Id, cancellationToken);

            if (existingOrder == null)
            {
                throw new Exception("Order not found.");
            }

            existingOrder.Copy(order);

            if (bookIds.Count > 0)
            {
                var queryable = dbContext.Set<Book>().AsQueryable();

                var booksFromDb = await queryable
                  .Where(b => bookIds.Contains(b.Id))
                  .ToListAsync(cancellationToken);

                existingOrder.Books = booksFromDb;
            }

            await dbContext.SaveChangesAsync(cancellationToken);
            return existingOrder;
        }
    }
}
