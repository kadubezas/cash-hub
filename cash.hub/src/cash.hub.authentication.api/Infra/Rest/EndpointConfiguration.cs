using cash.hub.authentication.api.Adapters.Inbound.Rest.Endpoint;

namespace cash.hub.authentication.api.infra.Rest;

public static class EndpointConfiguration
{
    public static void UseEndpointConfiguration(this WebApplication app)
    {        
        var prefix = "authentication/v1";

        app.MapHealthChecks("/health/liveness");
        app.MapHealthChecks("/health/readiness");
        app.MapHealthChecks("/health/startup");

        app.MapGroup(prefix)
            .MapRegisterEndpoint()
            .WithTags("Users");
        
        app.MapGroup(prefix)
            .MapAuthenticateEndpoint()
            .WithTags("Authentication");
    }
}