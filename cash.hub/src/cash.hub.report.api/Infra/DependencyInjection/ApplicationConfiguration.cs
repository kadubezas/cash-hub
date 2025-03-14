using cash.hub.report.api.Application.UseCases;

namespace cash.hub.report.api.Infra.DependencyInjection;

public static class ApplicationConfiguration
{
    public static void AddApplicationConfiguration(this IServiceCollection services)
    {
        services.AddScoped<ITransactionUseCase, TransactionsUseCase>();
    }
}