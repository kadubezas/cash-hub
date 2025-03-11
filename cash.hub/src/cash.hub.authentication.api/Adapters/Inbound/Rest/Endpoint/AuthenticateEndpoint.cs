using cash.hub.authentication.api.Adapters.Inbound.Rest.Response;
using cash.hub.authentication.api.Adapters.Inbound.Rest.Resquest;
using Microsoft.AspNetCore.Mvc;

namespace cash.hub.authentication.api.Adapters.Inbound.Rest.Endpoint;

public static class AuthenticateEndpoint
{
    public static RouteGroupBuilder MapAuthenticateEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/authenticate", AuthenticateAsync)
            .Produces<AuthenticateResponse>(200)
            .Produces<ErrorResponse>(400)
            .Produces<ErrorResponse>(500)
            //.AddEndpointFilter<ValidationFilter<>>()
            .WithOpenApi(); 
        return group;  
    }

    private static async Task<IResult> AuthenticateAsync([FromBody] AuthenticateRequest authenticateRequest)
    {
        return await Task.FromResult(Results.Ok());
    }
}