using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Filters;
using LibraryShopEntities.Repositories.Library;

namespace LibraryApi.Services
{
    public class LibraryEntityService<TEntity> : ILibraryEntityService<TEntity> where TEntity : BaseLibraryEntity
    {
        protected readonly ILibraryEntityRepository<TEntity> repository;

        public LibraryEntityService(ILibraryEntityRepository<TEntity> repository)
        {
            this.repository = repository;
        }

        public virtual async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await repository.GetByIdAsync(id, cancellationToken);
        }
        public virtual async Task<IEnumerable<TEntity>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken)
        {
            return await repository.GetByIdsAsync(ids, cancellationToken);
        }
        public virtual async Task<IEnumerable<TEntity>> GetPaginatedAsync(LibraryFilterRequest req, CancellationToken cancellationToken)
        {
            return await repository.GetPaginatedAsync(req, cancellationToken);
        }
        public virtual async Task<int> GetItemTotalAmountAsync(LibraryFilterRequest req, CancellationToken cancellationToken)
        {
            return await repository.GetItemTotalAmountAsync(req, cancellationToken);
        }
        public virtual async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            return await repository.CreateAsync(entity, cancellationToken);
        }
        public virtual async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            var entityInDb = await repository.GetByIdAsync(entity.Id, cancellationToken);

            if (entityInDb == null) throw new InvalidOperationException($"{typeof(TEntity).Name} is not found.");

            entityInDb.Copy(entity);

            return await repository.UpdateAsync(entityInDb, cancellationToken);
        }
        public virtual async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var entityInDb = await repository.GetByIdAsync(id, cancellationToken);
            if (entityInDb != null)
            {
                await repository.DeleteAsync(entityInDb, cancellationToken);
            }
        }
    }
}