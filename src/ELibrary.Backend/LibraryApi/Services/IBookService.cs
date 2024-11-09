using LibraryShopEntities.Domain.Entities.Library;

namespace LibraryApi.Services
{
    public interface IBookService : ILibraryEntityService<Book>
    {
        public Task RaisePopularityAsync(List<int> ids, CancellationToken cancellationToken);
        public Task ChangeBookStockAmount(Dictionary<int, int> changeRequests, CancellationToken cancellationToken);
    }
}