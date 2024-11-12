namespace Caching.Services
{
    public interface ICacheService
    {
        public ValueTask<string?> GetAsync<T>(string key);
        public Task SetAsync(string key, string value, TimeSpan duration);
        public ValueTask<bool> RemoveKeyAsync(string key);
    }
}
