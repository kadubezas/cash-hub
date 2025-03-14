using cash.hub.report.api.Adapters.Inbound.Rest.Responses.ValueObjects;

namespace cash.hub.report.api.Adapters.Inbound.Rest.Responses;

public record GetTransactionsResponse
{
    public List<TransationResponse> Transactions { get; set; } = new();
    public int TotalItems { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}