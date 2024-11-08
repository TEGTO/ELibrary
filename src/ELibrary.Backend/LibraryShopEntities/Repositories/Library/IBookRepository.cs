using LibraryShopEntities.Domain.Entities.Library;

namespace LibraryShopEntities.Repositories.Library
{
    public interface IBookRepository : ILibraryEntityRepository<Book>
    {
        public Task<IEnumerable<BookPopularity>> GetPopularitiesByIdsAsync(List<int> ids, CancellationToken cancellationToken);
        public Task UpdatePopularityRangeAsync(IEnumerable<BookPopularity> popularities, CancellationToken cancellationToken);
    }
}