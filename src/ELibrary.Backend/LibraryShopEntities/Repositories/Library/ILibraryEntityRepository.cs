using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Filters;

namespace LibraryShopEntities.Repositories.Library
{
    public interface ILibraryEntityRepository<TEntity> where TEntity : BaseLibraryEntity
    {
        public Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken);
        public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);
        public Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken);
        public Task<IEnumerable<TEntity>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken);
        public Task<int> GetItemTotalAmountAsync(LibraryFilterRequest req, CancellationToken cancellationToken);
        public Task<IEnumerable<TEntity>> GetPaginatedAsync(LibraryFilterRequest req, CancellationToken cancellationToken);
        public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
        public Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
    }
}