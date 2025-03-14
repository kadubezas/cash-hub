using cash.hub.register.api.Adapters.Outbound.Repository;
using cash.hub.register.api.Domain.Ports;

namespace cash.hub.register.api.Infra.DependencyInjection;

public static class DomainConfiguration
{
    public static void AddDomainConfiguration(this IServiceCollection services)
    {
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<ICashRegisterRepository, CashRegisterRepository>();
    }
}