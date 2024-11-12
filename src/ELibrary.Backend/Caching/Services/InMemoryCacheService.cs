using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Caching.Services
{
    public class InMemoryCacheService : ICacheService
    {
        private readonly IMemoryCache memoryCache;
        private readonly ILogger<InMemoryCacheService> logger;

        public InMemoryCacheService(IMemoryCache memoryCache, ILogger<InMemoryCacheService> logger)
        {
            this.memoryCache = memoryCache;
            this.logger = logger;
        }

        public ValueTask<string?> GetAsync<T>(string key)
        {
            memoryCache.TryGetValue(key, out string? value);
            return ValueTask.FromResult(value);
        }
        public Task SetAsync(string key, string value, TimeSpan duration)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = duration
            };
            memoryCache.Set(key, value, cacheEntryOptions);
            return Task.CompletedTask;
        }
        public ValueTask<bool> RemoveKeyAsync(string key)
        {
            try
            {
                memoryCache.Remove(key);
                return ValueTask.FromResult(true);
            }
            catch (Exception e)
            {
                string message = $"Error occured while parsing: {key}";
                logger.LogError(e, message);
                return ValueTask.FromResult(false);
            }
        }
    }
}
