using LibraryApi.Domain.Dtos;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Repositories;

namespace LibraryApi.Services
{
    public class LibraryEntityService<TEntity> : ILibraryEntityService<TEntity> where TEntity : BaseLibraryEntity
    {
        protected readonly IDatabaseRepository<LibraryDbContext> repository;

        public LibraryEntityService(IDatabaseRepository<LibraryDbContext> repository)
        {
            this.repository = repository;
        }

        public virtual async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<TEntity>(cancellationToken);

            return await queryable.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
        public virtual async Task<IEnumerable<TEntity>> GetByIdsAsync(List<int> ids, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<TEntity>(cancellationToken);

            return await queryable.AsNoTracking().Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);
        }
        public virtual async Task<IEnumerable<TEntity>> GetPaginatedAsync(LibraryFilterRequest req, CancellationToken cancellationToken)
        {
            var list = new List<TEntity>();
            var queryable = await repository.GetQueryableAsync<TEntity>(cancellationToken);

            list.AddRange(await queryable
                                     .AsNoTracking()
                                     .Where(x => x.Name.Contains(req.ContainsName))
                                     .OrderByDescending(b => b.Id)
                                     .Skip((req.PageNumber - 1) * req.PageSize)
                                     .Take(req.PageSize)
                                     .ToListAsync(cancellationToken));

            return list;
        }
        public virtual async Task<int> GetItemTotalAmountAsync(LibraryFilterRequest req, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<TEntity>(cancellationToken);

            return await queryable.AsNoTracking().Where(x => x.Name.Contains(req.ContainsName)).CountAsync(cancellationToken);
        }
        public virtual async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            return await repository.AddAsync(entity, cancellationToken);
        }
        public virtual async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<TEntity>(cancellationToken);
            var entityInDb = await queryable.FirstAsync(x => x.Id == entity.Id, cancellationToken);
            entityInDb.Copy(entity);
            return await repository.UpdateAsync(entityInDb, cancellationToken);
        }
        public virtual async Task DeleteByIdAsync(int id, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<TEntity>(cancellationToken);
            var entityInDb = await queryable.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entityInDb != null)
            {
                await repository.DeleteAsync(entityInDb, cancellationToken);
            }
        }
    }
}