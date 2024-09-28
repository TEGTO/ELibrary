using LibraryShopEntities.Domain.Entities.Shop;
using Shared.Dtos;

namespace ShopApi.Features.StockBookOrderFeature.Services
{
    public interface IStockBookOrderService
    {
        public Task<StockBookOrder> AddStockBookOrderAsync(StockBookOrder stockBookOrder, CancellationToken cancellationToken);
        public Task<StockBookOrder> AddStockBookOrderAsyncFromCanceledOrderAsync(Order order, StockBookOrderType type, CancellationToken cancellationToken);
        public Task<StockBookOrder> AddStockBookOrderAsyncFromOrderAsync(Order order, StockBookOrderType type, CancellationToken cancellationToken);
        public Task<IEnumerable<StockBookOrder>> GetPaginatedStockBookOrdersAsync(PaginationRequest pagination, CancellationToken cancellationToken);
        public Task<int> GetStockBookAmountAsync(CancellationToken cancellationToken);
        public Task<StockBookOrder?> GetStockBookOrderByIdAsync(int id, CancellationToken cancellationToken);
    }
}