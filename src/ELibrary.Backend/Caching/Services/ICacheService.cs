namespace Caching.Services
{
    public interface ICacheService
    {
        public T? Get<T>(string key);
        public void Set<T>(string key, T value, TimeSpan duration);
    }
}
