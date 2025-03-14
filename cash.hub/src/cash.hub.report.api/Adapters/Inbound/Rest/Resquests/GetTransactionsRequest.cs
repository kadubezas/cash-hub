namespace cash.hub.report.api.Adapters.Inbound.Rest.Resquests;

public record GetTransactionsRequest
{
    public DateTime Date { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}