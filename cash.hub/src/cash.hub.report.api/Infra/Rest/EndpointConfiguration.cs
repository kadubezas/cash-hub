using cash.hub.report.api.Adapters.Inbound.Rest.Endpoint;

namespace cash.hub.report.api.Infra.Rest;

public static class EndpointConfiguration
{
    public static void UseEndpointConfiguration(this WebApplication app)
    {        
        var prefix = "cash/hub/v1/";

        app.MapHealthChecks("/health/liveness");
        app.MapHealthChecks("/health/readiness");
        app.MapHealthChecks("/health/startup");

        app.MapGroup(prefix)
            .MapGetTransactionsEndpoint()
            .WithTags("Transaction");
        
    }
}