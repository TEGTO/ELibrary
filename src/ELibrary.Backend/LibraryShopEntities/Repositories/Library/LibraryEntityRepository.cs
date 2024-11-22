using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Filters;
using Microsoft.EntityFrameworkCore;
using Shared.Repositories;

namespace LibraryShopEntities.Repositories.Library
{
    public class LibraryEntityRepository<TEntity> : ILibraryEntityRepository<TEntity> where TEntity : BaseLibraryEntity
    {
        protected readonly IDatabaseRepository<LibraryDbContext> repository;

        public LibraryEntityRepository(IDatabaseRepository<LibraryDbContext> repository)
        {
            this.repository = repository;
        }

        public virtual async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<TEntity>(cancellationToken);
            return await queryable.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
        public virtual async Task<IEnumerable<TEntity>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<TEntity>(cancellationToken);
            return await queryable.AsNoTracking().Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);
        }
        public virtual async Task<IEnumerable<TEntity>> GetPaginatedAsync(LibraryFilterRequest req, CancellationToken cancellationToken)
        {
            var list = new List<TEntity>();
            var queryable = await repository.GetQueryableAsync<TEntity>(cancellationToken);

            var query = ApplyFilter(queryable, req);
            query = ApplyOrdering(query);

            list.AddRange(await ApplyPagination(query, req)
                .ToListAsync(cancellationToken));

            return list;
        }
        public virtual async Task<int> GetItemTotalAmountAsync(LibraryFilterRequest req, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<TEntity>(cancellationToken);
            var filteredQuery = ApplyFilter(queryable, req);

            return await filteredQuery.CountAsync(cancellationToken);
        }
        public virtual async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            return await repository.AddAsync(entity, cancellationToken);
        }
        public virtual async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            return await repository.UpdateAsync(entity, cancellationToken);
        }
        public virtual async Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        {
            await repository.UpdateRangeAsync(entities, cancellationToken);
        }
        public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await repository.DeleteAsync(entity, cancellationToken);
        }
        protected virtual IQueryable<TEntity> ApplyFilter(IQueryable<TEntity> query, LibraryFilterRequest req)
        {
            if (!string.IsNullOrEmpty(req.ContainsName))
            {
                query = query.Where(x => x.Name.Contains(req.ContainsName));
            }
            return query;
        }
        protected virtual IQueryable<TEntity> ApplyOrdering(IQueryable<TEntity> query)
        {
            return query.OrderByDescending(x => x.Id);
        }
        protected virtual IQueryable<TEntity> ApplyPagination(IQueryable<TEntity> query, LibraryFilterRequest req)
        {
            int skip = (req.PageNumber - 1) * req.PageSize;
            return query.Skip(skip).Take(req.PageSize);
        }
    }
}