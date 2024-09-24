﻿using LibraryShopEntities.Data;
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
            return
                await GetQueryableOrderWithBook(queryable)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
        public async Task<int> GetOrderAmountAsync(string clientId, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Order>(cancellationToken);
            return
                await GetQueryableOrderWithBook(queryable)
                .Where(x => x.ClientId == clientId)
                .CountAsync();
        }
        public async Task<int> GetOrderAmountAsync(CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Order>(cancellationToken);
            return
                await GetQueryableOrderWithBook(queryable)
                .CountAsync();
        }
        public async Task<IEnumerable<Order>> GetPaginatedOrdersAsync(PaginationRequest pagination, CancellationToken cancellationToken)
        {
            List<Order> orders = new List<Order>();
            var queryable = await repository.GetQueryableAsync<Order>(cancellationToken);

            orders.AddRange(await GetQueryableOrderWithBook(queryable)
                                     .OrderByDescending(b => b.CreationTime)
                                     .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                                     .Take(pagination.PageSize)
                                     .ToListAsync(cancellationToken));
            return orders;
        }
        public async Task<IEnumerable<Order>> GetPaginatedOrdersAsync(string id, PaginationRequest pagination, CancellationToken cancellationToken)
        {
            List<Order> orders = new List<Order>();
            var queryable = await repository.GetQueryableAsync<Order>(cancellationToken);

            orders.AddRange(
            await GetQueryableOrderWithBook(queryable)
            .Where(t => t.ClientId == id)
            .OrderByDescending(b => b.CreationTime)
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
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

            var newOrder = await repository.AddAsync(order, cancellationToken);
            var orderQueryable = await repository.GetQueryableAsync<Order>(cancellationToken);

            return await GetQueryableOrderWithBook(orderQueryable).FirstAsync(x => x.Id == newOrder.Id, cancellationToken);
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
            await repository.UpdateAsync(orderInDb, cancellationToken);

            return await GetQueryableOrderWithBook(queryable).FirstAsync(x => x.Id == orderInDb.Id, cancellationToken);
        }
        public async Task DeleteOrderAsync(int id, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Order>(cancellationToken);
            var entityInDb = await queryable.FirstAsync(x => x.Id == id, cancellationToken);
            await repository.DeleteAsync(entityInDb, cancellationToken);
        }

        #endregion

        private IQueryable<Order> GetQueryableOrderWithBook(IQueryable<Order> queryable)
        {
            return
                queryable
                .AsSplitQuery()
                .AsNoTracking()
                .Include(x => x.OrderBooks)
                    .ThenInclude(book => book.Book)
                    .ThenInclude(book => book.Author)
                .Include(x => x.OrderBooks)
                    .ThenInclude(book => book.Book)
                    .ThenInclude(book => book.Publisher)
                .Include(x => x.OrderBooks)
                    .ThenInclude(book => book.Book)
            .ThenInclude(book => book.Genre);
        }
    }
}