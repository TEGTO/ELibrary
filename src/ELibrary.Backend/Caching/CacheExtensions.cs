﻿using Caching.Services;
using Microsoft.AspNetCore.OutputCaching;
using System.Reflection;
using System.Text.Json;

namespace Caching
{
    public static class CacheExtensions
    {
        public static async ValueTask<T?> TryGetAsync<T>(this ICacheService service, string key, CancellationToken cancellationToken)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            var value = await service.GetAsync(key, cancellationToken);
            if (string.IsNullOrEmpty(value))
            {
                return default;
            }

            try
            {
                return JsonSerializer.Deserialize<T>(value);
            }
            catch (JsonException)
            {
                return default;
            }
        }
        public static async Task<bool> TrySetAsync<T>(this ICacheService service, string key, T value, TimeSpan duration, CancellationToken cancellationToken)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            string serializedValue;
            try
            {
                serializedValue = JsonSerializer.Serialize(value);
                return await service.SetAsync(key, serializedValue, duration, cancellationToken);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void SetOutputCachePolicy(this OutputCacheOptions options, string name, TimeSpan? duration = null, bool useAuthId = false, Type? type = null)
        {
            if (type != null)
            {
                PropertyInfo[] properties = type.GetProperties();
                options.AddPolicy(name,
                    new OutputCachePolicy(
                        duration,
                        useAuthId,
                        properties.SelectMany(p => new[] { p.Name, p.Name.ToLower() }).ToArray()
                        )
                    );
            }
            else
            {
                options.AddPolicy(name,
                    new OutputCachePolicy(
                        duration,
                        useAuthId
                        )
                    );
            }
        }
    }
}
