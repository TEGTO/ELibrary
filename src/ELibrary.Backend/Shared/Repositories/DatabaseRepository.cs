using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Registry;
using Shared.Configurations;

namespace Shared.Repositories
{
    public class DatabaseRepository<TContext> : IDatabaseRepository<TContext> where TContext : DbContext
    {
        private readonly IDbContextFactory<TContext> dbContextFactory;
        private readonly ResiliencePipeline resiliencePipeline;
        public DatabaseRepository(IDbContextFactory<TContext> dbContextFactory, ResiliencePipelineProvider<string> resiliencePipelineProvider)
        {
            this.dbContextFactory = dbContextFactory;
            resiliencePipeline = resiliencePipelineProvider.GetPipeline(Configuration.REPOSITORY_RESILIENCE_PIPELINE);
        }

        #region IDatabaseRepository Members

        public async Task MigrateDatabaseAsync(CancellationToken cancellationToken)
        {
            await resiliencePipeline.ExecuteAsync(async ct =>
            {
                var dbContext = await CreateDbContextAsync(ct);
                await dbContext.Database.MigrateAsync(ct);
            }, cancellationToken);
        }

        public async Task<T> AddAsync<T>(T obj, CancellationToken cancellationToken) where T : class
        {
            return await resiliencePipeline.ExecuteAsync(async ct =>
            {
                var dbContext = await CreateDbContextAsync(ct);
                await dbContext.AddAsync(obj, ct);
                await dbContext.SaveChangesAsync(ct);
                return obj;
            }, cancellationToken);
        }

        public async Task<IQueryable<T>> GetQueryableAsync<T>(CancellationToken cancellationToken) where T : class
        {
            return await resiliencePipeline.ExecuteAsync(async ct =>
            {
                var dbContext = await CreateDbContextAsync(ct);
                return dbContext.Set<T>().AsQueryable();
            }, cancellationToken);
        }

        public async Task<T> UpdateAsync<T>(T obj, CancellationToken cancellationToken) where T : class
        {
            return await resiliencePipeline.ExecuteAsync(async ct =>
            {
                var dbContext = await CreateDbContextAsync(ct);
                dbContext.Update(obj);
                await dbContext.SaveChangesAsync(ct);
                return obj;
            }, cancellationToken);
        }

        public async Task DeleteAsync<T>(T obj, CancellationToken cancellationToken) where T : class
        {
            await resiliencePipeline.ExecuteAsync(async ct =>
            {
                var dbContext = await CreateDbContextAsync(ct);
                dbContext.Remove(obj);
                await dbContext.SaveChangesAsync(ct);
            }, cancellationToken);
        }

        #endregion

        #region Protected Helpers

        protected async Task<TContext> CreateDbContextAsync(CancellationToken cancellationToken)
        {
            return await dbContextFactory.CreateDbContextAsync(cancellationToken);
        }

        #endregion
    }
}
