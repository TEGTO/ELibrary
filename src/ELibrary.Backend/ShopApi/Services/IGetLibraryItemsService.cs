
namespace ShopApi.Services
{
    public interface IGetLibraryItemsService
    {
        public Task<IEnumerable<T>> GetByIdsAsync<T>(List<int> ids, string endpoint, CancellationToken cancellationToken);
    }
}