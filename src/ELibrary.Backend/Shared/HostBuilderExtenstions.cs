using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Json;

namespace Shared
{
    public static class HostBuilderExtenstions
    {
        public static IHostBuilder SerilogConfiguration(this IHostBuilder hostBuilder)
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
