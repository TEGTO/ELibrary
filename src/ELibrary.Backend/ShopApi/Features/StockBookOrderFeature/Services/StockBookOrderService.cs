using EventSourcing;
using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Shop;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Dtos;
using Shared.Repositories;
using ShopApi.Features.StockBookOrderFeature.Models;

namespace ShopApi.Features.StockBookOrderFeature.Services
{
    public class StockBookOrderService : IStockBookOrderService
    {
        private readonly IDatabaseRepository<ShopDbContext> repository;
        private readonly IEventDispatcher eventDispatcher;

        public StockBookOrderService(IDatabaseRepository<ShopDbContext> repository, IEventDispatcher eventDispatcher)
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

            return (await GetStockBookOrderByIdAsync(newStockBookOrder.Id, cancellationToken))!;
        }
        public async Task<StockBookOrder> AddStockBookOrderAsyncFromOrderAsync(Order order, StockBookOrderType type, CancellationToken cancellationToken)
        {
            return await CreateStockBookOrderFromOrderAsync(order, type, -1, cancellationToken);
        }
        public async Task<StockBookOrder> AddStockBookOrderAsyncFromCanceledOrderAsync(Order order, StockBookOrderType type, CancellationToken cancellationToken)
        {
            return await CreateStockBookOrderFromOrderAsync(order, type, 1, cancellationToken);
        }
        public async Task<StockBookOrder?> GetStockBookOrderByIdAsync(int id, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<StockBookOrder>(cancellationToken);

            return await queryable
                .AsSplitQuery()
                .AsNoTracking()
                .Include(x => x.Client)
                .Include(x => x.StockBookChanges)
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

        private async Task<StockBookOrder> CreateStockBookOrderFromOrderAsync(Order order, StockBookOrderType type, int changeMultiplier, CancellationToken cancellationToken)
        {
            var stockCreateRequest = new StockBookOrder
            {
                ClientId = order.ClientId,
                Type = type
            };

            foreach (var orderBook in order.OrderBooks)
            {
                stockCreateRequest.StockBookChanges.Add(new StockBookChange
                {
                    BookId = orderBook.BookId,
                    ChangeAmount = changeMultiplier * orderBook.BookAmount
                });
            }

            return await AddStockBookOrderAsync(stockCreateRequest, cancellationToken);
        }

        #endregion
    }
}
