using cash.hub.authentication.api.Adapters.Outbound.Repository;
using cash.hub.authentication.api.Domain.Ports;

namespace cash.hub.authentication.api.infra.DependecyInjection;

public static class DomainConfiguration
{
    public static void AddDomainConfiguration(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
    }
}