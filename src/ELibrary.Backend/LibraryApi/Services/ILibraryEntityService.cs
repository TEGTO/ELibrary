using LibraryApi.Domain.Entities;

namespace LibraryApi.Services
{
    public interface ILibraryEntityService<TEntity> where TEntity : BaseEntity
    {
        public Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken);
        public Task DeleteByIdAsync(int id, CancellationToken cancellationToken);
        public Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken);
        public Task<IEnumerable<TEntity>> GetPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
        public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    }
}