using LibraryApi.Data;
using LibraryApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Repositories;

namespace LibraryApi.Services
{
    public class LibraryEntityService<TEntity> : ILibraryEntityService<TEntity> where TEntity : BaseEntity
    {
        private readonly IDatabaseRepository<LibraryDbContext> repository;

        protected LibraryEntityService(IDatabaseRepository<LibraryDbContext> repository)
        {
            this.repository = repository;
        }

        public async Task<TEntity?> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            using (var dbContext = await CreateDbContextAsync(cancellationToken))
            {
                return await dbContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            }
        }

        public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            using (var dbContext = await CreateDbContextAsync(cancellationToken))
            {
                await dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
                return entity;
            }
        }

        public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            using (var dbContext = await CreateDbContextAsync(cancellationToken))
            {
                var entityInDb = await dbContext.Set<TEntity>().FirstAsync(x => x.Id == entity.Id, cancellationToken);
                entityInDb.Copy(entity);
                dbContext.Set<TEntity>().Update(entityInDb);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task DeleteByIdAsync(string id, CancellationToken cancellationToken)
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

        private async Task<LibraryDbContext> CreateDbContextAsync(CancellationToken cancellationToken)
        {
            return await repository.CreateDbContextAsync(cancellationToken);
        }
    }
}