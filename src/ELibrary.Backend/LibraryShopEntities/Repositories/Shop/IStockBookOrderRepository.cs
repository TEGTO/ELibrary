using LibraryShopEntities.Domain.Entities.Shop;
using Pagination;

namespace LibraryShopEntities.Repositories.Shop
{
    public interface IStockBookOrderRepository
    {
        public Task<StockBookOrder> AddStockBookOrderAsync(StockBookOrder stockBookOrder, CancellationToken cancellationToken);
        public Task<IEnumerable<StockBookOrder>> GetPaginatedStockBookOrdersAsync(PaginationRequest pagination, CancellationToken cancellationToken);
        public Task<int> GetStockBookOrderAmountAsync(CancellationToken cancellationToken);
        public Task<StockBookOrder?> GetStockBookOrderByIdAsync(int id, CancellationToken cancellationToken);
    }
}