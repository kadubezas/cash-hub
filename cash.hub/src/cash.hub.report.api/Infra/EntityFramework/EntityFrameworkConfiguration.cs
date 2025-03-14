using cash.hub.report.api.Adapters.Outbound.Repository.DataBaseContextConfiguration;
using Microsoft.EntityFrameworkCore;

namespace cash.hub.report.api.infra.EntityFramework;

public static class EntityFrameworkConfiguration
{
    public static void AddEntityFrameworkConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataBaseContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("CASH_HUB_DB")));
    }
}