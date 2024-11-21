using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Shared.Repositories;

namespace DatabaseControl
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task<IApplicationBuilder> ConfigureDatabaseAsync<TContext>(this IApplicationBuilder builder, CancellationToken cancellationToken) where TContext : DbContext
        {
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger>();
                var repository = services.GetRequiredService<IDatabaseRepository<TContext>>();
                try
                {
                    logger.Information("Applying database migrations...");
                    await repository.MigrateDatabaseAsync(cancellationToken);
                    logger.Information("Database migrations applied successfully.");
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "An error occurred while migrating the database.");
                }
            }
            return builder;
        }
    }
}
