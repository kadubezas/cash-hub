using cash.hub.authentication.api.Adapters.Inbound.Rest.Response;
using cash.hub.authentication.api.Adapters.Inbound.Rest.Resquest;
using cash.hub.authentication.api.Application.Common.Enums;
using cash.hub.authentication.api.Application.UseCases.TokenJwt;
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
            .WithOpenApi(); 
        return group;  
    }

    private static async Task<IResult> AuthenticateAsync([FromBody] UserRequest userRequest,
                                                         [FromServices] ITokenJwtUseCase jwtUseCase)
    {
        var returnApplication = await jwtUseCase.GenerateJwtTokenAsync(userRequest.UserName, userRequest.Password);
        
        if (!returnApplication.Success)
        {
            var responseError = new ErrorResponse()
            {
                Code = returnApplication.ErrorType == ErrorType.Business ? 400 : 500,
                Message = returnApplication.Message!
            };

            return Results.Json(responseError, statusCode: returnApplication.ErrorType == ErrorType.Business ? 400 : 500);
        }

        var response = new AuthenticateResponse
        {
            Token = returnApplication.Data.Token,
            Expiration = returnApplication.Data.ExpiresAt
        };
        
        return await Task.FromResult(Results.Ok(response));
    }
}