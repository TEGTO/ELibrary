using EventSourcing;
using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Shop;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using Shared.Repositories;
using ShopApi.Features.StockBookOrderFeature.Models;

namespace ShopApi.Features.StockBookOrderFeature.Services
{
    public class StockBookOrderService : IStockBookOrderService
    {
        private readonly IDatabaseRepository<LibraryShopDbContext> repository;
        private readonly IEventDispatcher eventDispatcher;

        public StockBookOrderService(IDatabaseRepository<LibraryShopDbContext> repository, IEventDispatcher eventDispatcher)
        {
            this.repository = repository;
            this.eventDispatcher = eventDispatcher;
        }

        #region IStockBookOrderService Members

        public async Task<StockBookOrder> AddStockBookOrderAsync(StockBookOrder stockBookOrder, CancellationToken cancellationToken)
        {
            stockBookOrder.TotalChangeAmount = stockBookOrder.StockBookChanges
               .Sum(change =>
               {
                   return change.ChangeAmount;
               });

            var newStockBookOrder = await repository.AddAsync(stockBookOrder, cancellationToken);

            var bookPriceUpdatedEvent = new BookStockAmountUpdatedEvent(newStockBookOrder);
            await eventDispatcher.DispatchAsync(bookPriceUpdatedEvent, cancellationToken);

            return newStockBookOrder;
        }
        public async Task<StockBookOrder> AddStockBookOrderAsyncFromOrderAsync(Order order, CancellationToken cancellationToken)
        {
            return await CreateStockBookOrderAsync(order, -1, cancellationToken);
        }
        public async Task<StockBookOrder> AddStockBookOrderAsyncFromCanceledOrderAsync(Order order, CancellationToken cancellationToken)
        {
            return await CreateStockBookOrderAsync(order, 1, cancellationToken);
        }
        public async Task<StockBookOrder?> GetStockBookOrderByIdAsync(int id, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<StockBookOrder>(cancellationToken);

            return await queryable
                .AsSplitQuery()
                .AsNoTracking()
                .Include(x => x.Client)
                .Include(x => x.StockBookChanges)
                .ThenInclude(x => x.Book)
                .OrderByDescending(b => b.CreatedAt)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
        public async Task<IEnumerable<StockBookOrder>> GetPaginatedStockBookOrdersAsync(PaginationRequest pagination, CancellationToken cancellationToken)
        {
            List<StockBookOrder> orders = new List<StockBookOrder>();
            var queryable = await repository.GetQueryableAsync<StockBookOrder>(cancellationToken);

            orders.AddRange(await queryable
                                    .AsSplitQuery()
                                    .AsNoTracking()
                                    .Include(x => x.Client)
                                    .Include(x => x.StockBookChanges)
                                    .OrderByDescending(b => b.CreatedAt)
                                    .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                                    .Take(pagination.PageSize)
                                    .ToListAsync(cancellationToken));
            return orders;
        }
        public async Task<int> GetStockBookAmountAsync(CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<StockBookOrder>(cancellationToken);
            return await queryable.CountAsync();
        }

        #endregion

        #region Private Members

        private async Task<StockBookOrder> CreateStockBookOrderAsync(Order order, int changeMultiplier, CancellationToken cancellationToken)
        {
            var stockCreateRequest = new StockBookOrder
            {
                ClientId = order.ClientId
            };

            foreach (var orderBook in order.OrderBooks)
            {
                stockCreateRequest.StockBookChanges.Add(new StockBookChange
                {
                    BookId = orderBook.BookId,
                    ChangeAmount = changeMultiplier * order.OrderAmount
                });
            }

            return await AddStockBookOrderAsync(stockCreateRequest, cancellationToken);
        }

        #endregion
    }
}
