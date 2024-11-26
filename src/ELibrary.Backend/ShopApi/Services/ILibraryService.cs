
using LibraryShopEntities.Domain.Entities.Shop;

namespace ShopApi.Services
{
    public interface ILibraryService
    {
        public Task<IEnumerable<T>> GetByIdsAsync<T>(IEnumerable<int> ids, string endpoint, CancellationToken cancellationToken);
        public Task RaiseBookPopularityByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken);
        public Task UpdateBookStockAmountAsync(IEnumerable<StockBookChange> changes, CancellationToken cancellationToken);
    }
}