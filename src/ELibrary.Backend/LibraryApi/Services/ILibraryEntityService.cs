using LibraryApi.Domain.Entities;

namespace LibraryApi.Services
{
    public interface ILibraryEntityService<TEntity> where TEntity : BaseEntity
    {
        public Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken);
        public Task DeleteByIdAsync(string id, CancellationToken cancellationToken);
        public Task<TEntity?> GetByIdAsync(string id, CancellationToken cancellationToken);
        public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    }
}