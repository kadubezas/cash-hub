using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace cash.hub.report.api.Adapters.Outbound.Cache;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDistributedCache _cache;
    private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public RedisCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }
    
    public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
    {
        var json = JsonSerializer.Serialize(value, _serializerOptions);
        await _cache.SetStringAsync(key, json, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expiration });
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var json = await _cache.GetStringAsync(key);
        return json != null ? JsonSerializer.Deserialize<T>(json, _serializerOptions) : default;
    }
}