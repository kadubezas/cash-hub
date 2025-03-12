using cash.hub.authentication.api.Application.Common;
using cash.hub.authentication.api.Application.Common.FactoryBaseReturn;
using cash.hub.authentication.api.Application.Services;
using cash.hub.authentication.api.Application.UseCases.TokenJwt;
using cash.hub.authentication.api.Application.UseCases.UserPort;

namespace cash.hub.authentication.api.infra.DependecyInjection;

public static class ApplicationConfiguration
{
    public static void AddApplicationConfiguration(this IServiceCollection services)
    {
        services.AddScoped<IUserUseCase, UserUseCase>();
        services.AddScoped<ITokenJwtUseCase, TokenJwtUseCase>();
        services.AddScoped<ITokenJwtUseCase, TokenJwtUseCase>();
        services.AddSingleton<IResponseFactory, ResponseFactory>();
        services.AddSingleton<IPasswordService, PasswordService>();
        services.AddSingleton<IGenerateTokenService, GenerateTokenService>();
    }
}