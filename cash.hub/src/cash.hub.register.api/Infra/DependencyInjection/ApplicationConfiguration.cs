using cash.hub.register.api.Application.Common.FactoryBaseReturn;
using cash.hub.register.api.Application.UseCases.Transactions;

namespace cash.hub.register.api.Infra.DependencyInjection;

public static class ApplicationConfiguration
{
    public static void AddApplicationConfiguration(this IServiceCollection services)
    {
        services.AddScoped<ITransactionUseCase, TransactionUseCase>();
        services.AddSingleton<IResponseFactory, ResponseFactory>();
    }
}