using cash.hub.authentication.api.Application.Common.Config;

namespace cash.hub.authentication.api.Infra.JwtConfig;

public static class JwtConfiguration
{
    public static void AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
    }
}