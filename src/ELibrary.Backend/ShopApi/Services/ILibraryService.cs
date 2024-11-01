
namespace ShopApi.Services
{
    public interface ILibraryService
    {
        public Task<IEnumerable<T>> GetByIdsAsync<T>(List<int> ids, string endpoint, CancellationToken cancellationToken);
        public Task RaiseBookPopularityByIdsAsyn(List<int> ids, CancellationToken cancellationToken);
    }
}