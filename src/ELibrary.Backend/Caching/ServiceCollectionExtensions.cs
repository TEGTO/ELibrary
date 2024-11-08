using Caching.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;

namespace Caching
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCachingHelper(this IServiceCollection services)
        {
            services.AddSingleton<ICachingHelper, CachingHelper>();

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

            return services;
        }
    }
}
