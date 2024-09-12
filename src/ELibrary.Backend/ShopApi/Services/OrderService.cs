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
            .ToListAsync(cancellationToken));

            return orders;
        }
        public async Task<Order> CreateOrderAsync(Order order, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Book>(cancellationToken);
            var books = await queryable.Where(x => order.Books.Any(y => y.Id == x.Id)).ToListAsync(cancellationToken);

            order.Books = books;

            return await repository.AddAsync(order, cancellationToken);
        }
        public async Task<bool> CheckOrderAsync(string clientId, int id, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Order>(cancellationToken);
            return await queryable.AsNoTracking().AnyAsync(x => x.Id == id && x.ClientId == clientId, cancellationToken);
        }
        public async Task<Order> UpdateOrderAsync(Order order, CancellationToken cancellationToken)
        {
            var queryableOrders = await repository.GetQueryableAsync<Order>(cancellationToken);
            var orderInDb = await queryableOrders.FirstAsync(x => x.Id == order.Id, cancellationToken);

            orderInDb.Copy(order);

            if (orderInDb.OrderStatus == OrderStatus.InProcessing)
            {
                var queryableBooks = await repository.GetQueryableAsync<Book>(cancellationToken);
                var books = await queryableBooks.Where(x => order.Books.Any(y => y.Id == x.Id)).ToListAsync(cancellationToken);

                orderInDb.Books = books;
            }

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