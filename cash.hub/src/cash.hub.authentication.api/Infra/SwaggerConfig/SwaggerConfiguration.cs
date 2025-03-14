using Microsoft.OpenApi.Models;

namespace cash.hub.authentication.api.Infra.SwaggerConfig;

public static class SwaggerConfiguration
{
    public static void AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Cash Hub Authentication",
                Description = "API com objetivo cadastrar e autenticar usuários",
            });
        });
    }
}