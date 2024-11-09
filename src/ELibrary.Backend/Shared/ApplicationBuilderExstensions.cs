using ExceptionHandling;
using Microsoft.AspNetCore.Builder;
using Serilog;

namespace Shared
{
    public static class ApplicationBuilderExstensions
    {
        public static IApplicationBuilder UseSharedMiddlewares(this IApplicationBuilder builder)
        {
            builder.UseExceptionMiddleware();
            builder.UseSerilogRequestLogging();

            return builder;
        }
    }
}