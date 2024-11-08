using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Json;

namespace Logging
{
    public static class HostBuilderExtenstions
    {
        public static IHostBuilder AddLogging(this IHostBuilder hostBuilder)
        {
            hostBuilder.UseSerilog((context, loggerConfig) =>
            {
                loggerConfig.WriteTo.Console();
                loggerConfig.WriteTo.File(new JsonFormatter(), "logs/applogs-.txt", rollingInterval: RollingInterval.Day);
            });

            return hostBuilder;
        }
    }
}
