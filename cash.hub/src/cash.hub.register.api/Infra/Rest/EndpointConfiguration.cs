using cash.hub.register.api.Adapters.Inbound.Rest.Endpoints;

namespace cash.hub.register.api.Infra.Rest;

public static class EndpointConfiguration
{
    public static void UseEndpointConfiguration(this WebApplication app)
    {        
        var prefix = "cash/hub/v1/";

        app.MapHealthChecks("/health/liveness");
        app.MapHealthChecks("/health/readiness");
        app.MapHealthChecks("/health/startup");

        app.MapGroup(prefix)
            .MapRegisterTransactionEndpoint()
            .WithTags("Transaction");
        
    }
}