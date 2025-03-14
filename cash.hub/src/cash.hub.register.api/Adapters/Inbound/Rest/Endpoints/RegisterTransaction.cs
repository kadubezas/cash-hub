using cash.hub.register.api.Adapters.Inbound.Rest.Common.Enums;
using cash.hub.register.api.Adapters.Inbound.Rest.Requests;
using cash.hub.register.api.Adapters.Inbound.Rest.Responses;
using cash.hub.register.api.Application.Common.Enums;
using cash.hub.register.api.Application.Dto.Inputs;
using cash.hub.register.api.Application.Dto.Outputs;
using cash.hub.register.api.Application.UseCases.Transactions;
using cash.hub.register.api.Domain.Entities.Enums;
using Microsoft.AspNetCore.Mvc;

namespace cash.hub.register.api.Adapters.Inbound.Rest.Endpoints;

public static class RegisterTransaction
{
    public static RouteGroupBuilder MapRegisterTransactionEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("transaction/register", RegisterTransactionAsync)
            .Produces<RegisterTransactionResponse>(201)
            .Produces<ErrorResponse>(400)
            .Produces<ErrorResponse>(500)
            .RequireAuthorization()
            .WithOpenApi(); 
        return group;  
    }

    private static async Task<IResult> RegisterTransactionAsync([FromBody] RegisterTransactionRequest request,
                                                                [FromServices] ITransactionUseCase useCase)
    {
        var input = new TransactionInput()
        {
            Type = (TransactionType) request.Type,
            PaymentMethod = (PaymentMethod) request.PaymentMethod,
            Installments = request.Installments,
            Notes = request.Notes,
            Value = request.Value,
            CashRegisterId = request.CashRegisterId
        };
        
        var baseReturn = await useCase.RegisterAsync(input);
        
        if (baseReturn.Data is null && !baseReturn.Success)
        {
            var responseError = new ErrorResponse()
            {
                Code = baseReturn.ErrorType == ErrorType.Business ? 400 : 500,
                Message = baseReturn.Message!
            };

            return Results.Json(responseError, statusCode: baseReturn.ErrorType == ErrorType.Business ? 400 : 500);
        }


        var response = new RegisterTransactionResponse()
        {
            TransactionId = baseReturn.Data!.TransactionId,
            Status = (TransactionStatusRequest) baseReturn.Data.Status,
            CreatedAt = baseReturn.Data.CreatedAt
        };
        
        return Results.Created("", response);

    }
}