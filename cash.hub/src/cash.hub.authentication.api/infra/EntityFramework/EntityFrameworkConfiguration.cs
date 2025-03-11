using cash.hub.authentication.api.Adapters.Outbound.Repository.DataBaseContext;
using Microsoft.EntityFrameworkCore;

namespace cash.hub.authentication.api.infra.EntityFramework;

public static class EntityFrameworkConfiguration
{
    public static void AddEntityFrameworkConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataBaseContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("AUTHENTICATION_DB")));
        
        
    }

    public static void UseEntityFrameworkConfiguration(this WebApplication app)
    {
        var scope = app.Services.CreateScope();
        
        var dbContext = scope.ServiceProvider.GetRequiredService<DataBaseContext>();
        dbContext.Database.Migrate();
        
        DbInitializer.Seed(dbContext);
    }
}