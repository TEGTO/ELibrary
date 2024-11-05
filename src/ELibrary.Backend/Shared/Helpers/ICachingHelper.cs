using Microsoft.AspNetCore.Http;

namespace Shared.Helpers
{
    public interface ICachingHelper
    {
        public string GetCacheKey(string prefix, HttpContext context);
    }
}