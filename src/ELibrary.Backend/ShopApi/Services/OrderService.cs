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

        public async Task<Order?> GetOrderByIdAsync(int id, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Order>(cancellationToken);
            return await queryable.AsNoTracking().Include(x => x.OrderBooks).ThenInclude(x => x.Book).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
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
                                     .Include(x => x.OrderBooks)
                                     .ThenInclude(x => x.Book)
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
            .Include(x => x.OrderBooks)
            .ThenInclude(x => x.Book)
            .ToListAsync(cancellationToken));

            return orders;
        }
        public async Task<Order> CreateOrderAsync(Order order, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Book>(cancellationToken);

            order.OrderAmount = order.OrderBooks.Sum(x => x.BookAmount);

            var bookIds = order.OrderBooks.Select(ob => ob.BookId).ToList();
            var booksInOrder = await queryable
                .Where(book => bookIds.Contains(book.Id))
                .ToListAsync(cancellationToken);

            order.TotalPrice = order.OrderBooks
                .Sum(orderBook =>
                {
                    var book = booksInOrder.FirstOrDefault(b => b.Id == orderBook.BookId);
                    return book != null ? orderBook.BookAmount * book.Price : 0;
                });

            return await repository.AddAsync(order, cancellationToken);
        }
        public async Task<bool> CheckOrderAsync(string clientId, int id, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Order>(cancellationToken);
            return await queryable.AsNoTracking().AnyAsync(x => x.Id == id && x.ClientId == clientId, cancellationToken);
        }
        public async Task<Order> UpdateOrderAsync(Order order, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Order>(cancellationToken);
            var orderInDb = await queryable.FirstOrDefaultAsync(x => x.Id == order.Id);

            if (orderInDb == null)
            {
                throw new Exception("Order not found.");
            }

            orderInDb.Copy(order);
            return await repository.UpdateAsync(orderInDb, cancellationToken);
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