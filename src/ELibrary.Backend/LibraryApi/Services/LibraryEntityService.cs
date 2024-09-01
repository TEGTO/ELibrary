using LibraryApi.Data;
using LibraryApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Repositories;

namespace LibraryApi.Services
{
    public class LibraryEntityService<TEntity> : ILibraryEntityService<TEntity> where TEntity : BaseEntity
    {
        private readonly IDatabaseRepository<LibraryDbContext> repository;

        public LibraryEntityService(IDatabaseRepository<LibraryDbContext> repository)
        {
            this.repository = repository;
        }

        public virtual async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            using (var dbContext = await CreateDbContextAsync(cancellationToken))
            {
                return await dbContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var list = new List<TEntity>();
            using (var dbContext = await CreateDbContextAsync(cancellationToken))
            {
                list.AddRange(await dbContext.Set<TEntity>()
                                      .AsNoTracking()
                                      .OrderByDescending(b => b.Id)
                                      .Skip((pageNumber - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToListAsync(cancellationToken));
            }
            return list;
        }

        public virtual async Task<int> GetItemTotalAmountAsync(CancellationToken cancellationToken)
        {
            using (var dbContext = await CreateDbContextAsync(cancellationToken))
            {
                return await dbContext.Set<TEntity>().AsNoTracking().CountAsync(cancellationToken);
            }
        }

        public virtual async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            using (var dbContext = await CreateDbContextAsync(cancellationToken))
            {
                await dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
                return entity;
            }
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            using (var dbContext = await CreateDbContextAsync(cancellationToken))
            {
                var entityInDb = await dbContext.Set<TEntity>().FirstAsync(x => x.Id == entity.Id, cancellationToken);
                entityInDb.Copy(entity);
                dbContext.Set<TEntity>().Update(entityInDb);
                await dbContext.SaveChangesAsync(cancellationToken);
                return entityInDb;
            }
        }

        public virtual async Task DeleteByIdAsync(int id, CancellationToken cancellationToken)
        {
            using (var dbContext = await CreateDbContextAsync(cancellationToken))
            {
                var entityInDb = await dbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
                if (entityInDb != null)
                {
                    dbContext.Set<TEntity>().Remove(entityInDb);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            }
        }

        protected async Task<LibraryDbContext> CreateDbContextAsync(CancellationToken cancellationToken)
        {
            return await repository.CreateDbContextAsync(cancellationToken);
        }

    }
}