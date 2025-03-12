using cash.hub.authentication.api.Adapters.Inbound.Rest.Response;
using cash.hub.authentication.api.Adapters.Inbound.Rest.Resquest;
using cash.hub.authentication.api.Application.Common;
using cash.hub.authentication.api.Application.Common.Dto;
using cash.hub.authentication.api.Application.Common.Enums;
using cash.hub.authentication.api.Application.UseCases.UserPort;
using Microsoft.AspNetCore.Mvc;

namespace cash.hub.authentication.api.Adapters.Inbound.Rest.Endpoint;

public static class RegisterUserEndpoint
{
    public static RouteGroupBuilder MapRegisterEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("user/register", RegisterUserAsync)
            .Produces<RegisterUserResponse>(201)
            .Produces<ErrorResponse>(400)
            .Produces<ErrorResponse>(500)
            .WithOpenApi(); 
        return group;  
    }

    private static async Task<IResult> RegisterUserAsync([FromBody] UserRequest userRequest,
                                                         [FromServices] IUserUseCase useCase)
    {
        
        var returnApplication = await useCase.RegisterAsync(userRequest.UserName, userRequest.Password);

        if (!returnApplication.Success)
        {
            var responseError = new ErrorResponse()
            {
                Code = returnApplication.ErrorType == ErrorType.Business ? 400 : 500,
                Message = returnApplication.Message!
            };

            return Results.Json(responseError, statusCode: returnApplication.ErrorType == ErrorType.Business ? 400 : 500);
        }

        var succsessMessage = new RegisterUserResponse()
        {
            Message = returnApplication.Message
        };
        
        return await Task.FromResult(Results.Created("",succsessMessage));
    }
}