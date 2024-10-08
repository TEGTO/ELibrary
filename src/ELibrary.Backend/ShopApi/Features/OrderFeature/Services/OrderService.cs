using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Domain.Entities.Shop;
using Microsoft.EntityFrameworkCore;
using Shared.Repositories;
using ShopApi.Features.OrderFeature.Dtos;

namespace ShopApi.Features.OrderFeature.Services
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
            return
                await GetQueryableOrder(queryable)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
        public async Task<int> GetOrderAmountAsync(GetOrdersFilter filter, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Order>(cancellationToken);
            return
                await ApplyFilter(GetQueryableOrder(queryable), filter)
                        .CountAsync();
        }
        public async Task<IEnumerable<Order>> GetPaginatedOrdersAsync(GetOrdersFilter filter, CancellationToken cancellationToken)
        {
            List<Order> orders = new List<Order>();
            var queryable = await repository.GetQueryableAsync<Order>(cancellationToken);

            orders.AddRange(await ApplyFilter(GetQueryableOrder(queryable), filter)
                                     .OrderByDescending(b => b.CreatedAt)
                                     .Skip((filter.PageNumber - 1) * filter.PageSize)
                                     .Take(filter.PageSize)
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
                    var book = booksInOrder.First(b => b.Id == orderBook.BookId);
                    return book != null ? orderBook.BookAmount * book.Price : 0;
                });

            var newOrder = await repository.AddAsync(order, cancellationToken);
            var orderQueryable = await repository.GetQueryableAsync<Order>(cancellationToken);

            return await GetQueryableOrder(orderQueryable).FirstAsync(x => x.Id == newOrder.Id, cancellationToken);
        }
        public async Task<Order> UpdateOrderAsync(Order order, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Order>(cancellationToken);
            var orderInDb = await queryable.FirstOrDefaultAsync(x => x.Id == order.Id);

            if (orderInDb == null)
            {
                throw new InvalidOperationException("Order is not found.");
            }

            if (orderInDb.OrderStatus != OrderStatus.InProcessing)
            {
                throw new InvalidOperationException("Orders that are not in processing cannot be changed!");
            }

            orderInDb.Copy(order);
            await repository.UpdateAsync(orderInDb, cancellationToken);

            return await GetQueryableOrder(queryable).FirstAsync(x => x.Id == orderInDb.Id, cancellationToken);
        }
        public async Task DeleteOrderAsync(int id, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Order>(cancellationToken);
            var entityInDb = await queryable.FirstAsync(x => x.Id == id, cancellationToken);
            await repository.DeleteAsync(entityInDb, cancellationToken);
        }

        #endregion

        #region Private Helpers

        private IQueryable<Order> GetQueryableOrder(IQueryable<Order> queryable)
        {
            return
                queryable
                .AsSplitQuery()
                .AsNoTracking()
                .Include(x => x.OrderBooks)
                    .ThenInclude(book => book.Book)
                .Include(x => x.Client);
        }
        private IQueryable<Order> ApplyFilter(IQueryable<Order> queryable, GetOrdersFilter filter)
        {
            if (filter.ClientId != null)
            {
                queryable = queryable.Where(t => t.ClientId == filter.ClientId);
            }

            return queryable;
        }

        #endregion
    }
}