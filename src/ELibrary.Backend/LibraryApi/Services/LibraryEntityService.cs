using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Dtos;
using LibraryShopEntities.Domain.Entities.Library;
using Microsoft.EntityFrameworkCore;
using Shared.Repositories;

namespace LibraryShopEntities.Services
{
    public class LibraryEntityService<TEntity> : ILibraryEntityService<TEntity> where TEntity : BaseLibraryEntity
    {
        protected readonly IDatabaseRepository<LibraryShopDbContext> repository;

        public LibraryEntityService(IDatabaseRepository<LibraryShopDbContext> repository)
        {
            this.repository = repository;
        }

        public virtual async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<TEntity>(cancellationToken);

            return await queryable.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public virtual async Task<IEnumerable<TEntity>> GetPaginatedAsync(LibraryPaginationRequest pagination, CancellationToken cancellationToken)
        {
            var list = new List<TEntity>();
            var queryable = await repository.GetQueryableAsync<TEntity>(cancellationToken);

            list.AddRange(await queryable
                                     .AsNoTracking()
                                     .Where(x => x.Name.Contains(pagination.ContainsName))
                                     .OrderByDescending(b => b.Id)
                                     .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                                     .Take(pagination.PageSize)
                                     .ToListAsync(cancellationToken));

            return list;
        }

        public virtual async Task<int> GetItemTotalAmountAsync(CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<TEntity>(cancellationToken);

            return await queryable.AsNoTracking().CountAsync(cancellationToken);
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