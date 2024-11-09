using EventSourcing;
using LibraryShopEntities.Domain.Entities.Shop;
using LibraryShopEntities.Repositories.Shop;
using Microsoft.EntityFrameworkCore;
using Pagination;
using ShopApi.Features.StockBookOrderFeature.Models;

namespace ShopApi.Features.StockBookOrderFeature.Services
{
    public class StockBookOrderService : IStockBookOrderService
    {
        private readonly IStockBookOrderRepository stockBookOrderRepository;
        private readonly IEventDispatcher eventDispatcher;

        public StockBookOrderService(IStockBookOrderRepository stockBookOrderRepository, IEventDispatcher eventDispatcher)
        {
            this.stockBookOrderRepository = stockBookOrderRepository;
            this.eventDispatcher = eventDispatcher;
        }

        #region IStockBookOrderService Members

        public async Task<StockBookOrder> AddStockBookOrderAsync(StockBookOrder stockBookOrder, CancellationToken cancellationToken)
        {
            stockBookOrder.TotalChangeAmount = stockBookOrder.StockBookChanges.Sum(change => change.ChangeAmount);

            var newStockBookOrder = await stockBookOrderRepository.AddStockBookOrderAsync(stockBookOrder, cancellationToken);

            var bookPriceUpdatedEvent = new BookStockAmountUpdatedEvent(newStockBookOrder);
            await eventDispatcher.DispatchAsync(bookPriceUpdatedEvent, cancellationToken);

            return (await stockBookOrderRepository.GetStockBookOrderByIdAsync(newStockBookOrder.Id, cancellationToken))!;
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
            return await stockBookOrderRepository.GetStockBookOrderByIdAsync(id, cancellationToken);
        }
        public async Task<IEnumerable<StockBookOrder>> GetPaginatedStockBookOrdersAsync(PaginationRequest pagination, CancellationToken cancellationToken)
        {
            return await stockBookOrderRepository.GetPaginatedStockBookOrdersAsync(pagination, cancellationToken);
        }
        public async Task<int> GetStockBookAmountAsync(CancellationToken cancellationToken)
        {
            return await stockBookOrderRepository.GetStockBookOrderAmountAsync(cancellationToken);
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
