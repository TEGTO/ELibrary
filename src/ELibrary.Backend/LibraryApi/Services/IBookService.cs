using LibraryShopEntities.Domain.Entities.Library;

namespace LibraryApi.Services
{
    public interface IBookService : ILibraryEntityService<Book>
    {
        public Task RaisePopularityAsync(IEnumerable<int> ids, CancellationToken cancellationToken);
        public Task ChangeBookStockAmount(Dictionary<int, int> changeRequests, CancellationToken cancellationToken);
    }
}