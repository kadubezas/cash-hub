namespace cash.hub.report.api.Adapters.Outbound.Cache;

public interface IRedisCacheService
{
    public Task SetAsync<T>(string key, T value, TimeSpan expiration);

    public Task<T?> GetAsync<T>(string key);
}