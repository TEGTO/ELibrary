using Microsoft.Extensions.Caching.Memory;

namespace Caching.Services
{
    public class InMemoryCacheService : ICacheService
    {
        private readonly IMemoryCache memoryCache;

        public InMemoryCacheService(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public T? Get<T>(string key)
        {
            memoryCache.TryGetValue(key, out T? value);
            return value;
        }

        public void Set<T>(string key, T value, TimeSpan duration)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = duration
            };
            memoryCache.Set(key, value, cacheEntryOptions);
        }
    }
}
