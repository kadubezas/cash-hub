using cash.hub.report.api.Adapters.Inbound.Rest.Common.Enums;
using cash.hub.report.api.Adapters.Inbound.Rest.Responses;
using cash.hub.report.api.Adapters.Inbound.Rest.Responses.ValueObjects;
using cash.hub.report.api.Adapters.Inbound.Rest.Resquests;
using cash.hub.report.api.Application.UseCases;
using cash.hub.report.api.Domain.Entities;
using cash.hub.report.api.Domain.Entities.Enums;
using Microsoft.AspNetCore.Mvc;

namespace cash.hub.report.api.Adapters.Inbound.Rest.Endpoint;

public static class GetTransactionsEndpoint
{
    public static RouteGroupBuilder MapGetTransactionsEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("transactions", GetTransactionsEndpointAsync)
             .Produces<GetTransactionsResponse>(200)
             .Produces<ErrorResponse>(400)
             .Produces<ErrorResponse>(500)
             //.RequireAuthorization()
            .WithOpenApi(); 
        return group;  
    }

    private static async Task<IResult> GetTransactionsEndpointAsync([AsParameters] GetTransactionsRequest request,
                                                                    [FromServices] ITransactionUseCase useCase)
    {
        var pageResponse = await useCase.GetTransactionsAsync(request.Date, request.Page, request.PageSize);

        var response = new GetTransactionsResponse
        {
            Transactions = pageResponse.Items.Select(MapToResponse).ToList(),
            TotalItems = pageResponse.TotalItems,
            PageSize = pageResponse.PageSize,
            Page = pageResponse.Page,
            TotalPages = pageResponse.TotalPages
        };
        
        return Results.Ok(response);
    }

    private static TransationResponse MapToResponse(Transaction transaction)
    {
        return new TransationResponse
        {
            TransactionId = transaction.TransactionId,
            Type = (TransactionTypeRequest)transaction.Type,
            Value = transaction.Value,
            CreatedAt = transaction.CreatedAt,
            Status = (TransactionStatusRequest)transaction.Status
        };
    }
}