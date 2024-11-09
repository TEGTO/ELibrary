
using LibraryShopEntities.Domain.Entities.Shop;

namespace ShopApi.Services
{
    public interface ILibraryService
    {
        public Task<IEnumerable<T>> GetByIdsAsync<T>(List<int> ids, string endpoint, CancellationToken cancellationToken);
        public Task RaiseBookPopularityByIdsAsync(List<int> ids, CancellationToken cancellationToken);
        public Task UpdateBookStockAmountAsync(List<StockBookChange> changes, CancellationToken cancellationToken);
    }
}