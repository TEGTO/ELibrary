using Microsoft.AspNetCore.Http;

namespace Shared.Helpers
{
    public class CachingHelper : ICachingHelper
    {
        public string GetCacheKey(string prefix, HttpContext context)
        {
            string ip = GetIPAddress(context) ?? Guid.NewGuid().ToString();
            return $"{prefix}_{ip}";
        }

        private static string? GetIPAddress(HttpContext context)
        {
            if (context.Request.Host.Host == "localhost")
            {
                return "127.0.0.1";
            }

            return context.GetServerVariable("HTTP_X_FORWARDED_FOR")
                ?? context.Connection.RemoteIpAddress?.ToString();
        }
    }
}
