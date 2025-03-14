using cash.hub.report.api.Adapters.Outbound.Cache;

namespace cash.hub.report.api.Infra.RedisConfig;

public static class RedisConfiguration
{
    public static void AddRedisConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConfig = configuration.GetSection("Redis");
        
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConfig["ConnectionString"];
            options.InstanceName = redisConfig["InstanceName"];
        });

        services.AddScoped<IRedisCacheService, RedisCacheService>();
    }
}