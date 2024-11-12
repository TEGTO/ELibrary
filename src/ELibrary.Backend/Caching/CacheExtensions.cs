using Caching.Services;
using System.Text.Json;

namespace Caching
{
    public static class CacheExtensions
    {
        public static async ValueTask<T?> GetDeserializedAsync<T>(this ICacheService service, string key)
        {
            var value = await service.GetAsync<T>(key);

            if (value == default)
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(value);
        }
        public static Task SetSerializedAsync<T>(this ICacheService service, string key, T value, TimeSpan duration)
        {
            return service.SetAsync(key, JsonSerializer.Serialize(value), duration);
        }
    }
}
