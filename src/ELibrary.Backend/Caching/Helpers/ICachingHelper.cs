using Microsoft.AspNetCore.Http;

namespace Caching.Helpers
{
    public interface ICachingHelper
    {
        public string GetCacheKey(string prefix, HttpContext context);
    }
}