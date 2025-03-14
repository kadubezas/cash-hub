using cash.hub.report.api.Adapters.Outbound.Repository;
using cash.hub.report.api.Domain.Ports;

namespace cash.hub.report.api.Infra.DependencyInjection;

public static class DomainConfiguration
{
    public static void AddDomainConfiguration(this IServiceCollection services)
    {
        services.AddScoped<ITransactionRepository, TransactionRepository>();
    }
}