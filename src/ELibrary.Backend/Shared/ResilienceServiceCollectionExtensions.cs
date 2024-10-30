using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.DependencyInjection;
using Shared.Configurations;
using Shared.Helpers;
using Shared.Repositories;

namespace Shared
{
    public static class ResilienceServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositoryPatternWithResilience<Context>(this IServiceCollection services, IConfiguration configuration) where Context : DbContext
        {
            var pipelineConfiguration = configuration.GetSection(SharedConfiguration.REPOSITORY_RESILIENCE_PIPELINE)
                                        .Get<ResiliencePipelineConfiguration>() ?? new ResiliencePipelineConfiguration();

            services.AddResiliencePipeline(SharedConfiguration.REPOSITORY_RESILIENCE_PIPELINE, (builder, context) =>
            {
                ConfigureResiliencePipeline(builder, context, pipelineConfiguration);
            });

            services.AddSingleton<IDatabaseRepository<Context>, DatabaseRepository<Context>>();

            return services;
        }
        public static IServiceCollection AddCustomHttpClientServiceWithResilience(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient(SharedConfiguration.HTTP_CLIENT_RESILIENCE_PIPELINE).AddStandardResilienceHandler();
            services.AddSingleton<IHttpHelper, HttpHelper>();
            return services;
        }
        public static IServiceCollection AddDefaultResiliencePipeline(this IServiceCollection services, IConfiguration configuration, string defaultName = "Default")
        {
            var pipelineConfiguration = configuration.GetSection(SharedConfiguration.DEFAULT_RESILIENCE_PIPELINE_SECTION)
                                        .Get<ResiliencePipelineConfiguration>() ?? new ResiliencePipelineConfiguration();

            services.AddResiliencePipeline(defaultName, (builder, context) =>
            {
                ConfigureResiliencePipeline(builder, context, pipelineConfiguration);
            });

            return services;
        }

        private static void ConfigureResiliencePipeline(ResiliencePipelineBuilder builder, AddResiliencePipelineContext<string> context, ResiliencePipelineConfiguration config)
        {
            builder.AddRetry(new()
            {
                Delay = TimeSpan.FromSeconds(config.RetryWaitInSeconds),
                MaxRetryAttempts = config.MaxRetryCount,
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                OnRetry = args =>
                {
                    var logger = context.ServiceProvider.GetService<ILogger>();
                    var str = $"Retry {args.AttemptNumber} due to {args.Outcome.Exception?.Message}. Waiting {args.Duration.TotalSeconds} seconds before next retry.";
                    logger?.LogWarning(str);
                    return default;
                }
            })
            .AddCircuitBreaker(new()
            {
                FailureRatio = config.CircuitFailureRatio,
                MinimumThroughput = config.CircuitMinimumThroughput,
                BreakDuration = TimeSpan.FromSeconds(config.CircuitWaitInSeconds),
                OnOpened = args =>
                {
                    var logger = context.ServiceProvider.GetService<ILogger>();
                    var str = $"Circuit breaker triggered. Circuit will be open for {args.BreakDuration.TotalSeconds} seconds due to {args.Outcome.Exception?.Message}.";
                    logger?.LogWarning(str);
                    return default;
                },
                OnClosed = args =>
                {
                    var logger = context.ServiceProvider.GetService<ILogger>();
                    var str = "Circuit breaker closed.";
                    logger?.LogWarning(str);
                    return default;
                }
            });
        }
    }
}
