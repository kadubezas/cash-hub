using cash.hub.report.api.Adapters.Inbound.Rest.Common.Enums;

namespace cash.hub.report.api.Adapters.Inbound.Rest.Responses.ValueObjects;

public record TransationResponse
{
    public Guid TransactionId { get; set; } = Guid.NewGuid(); 
    public TransactionTypeRequest Type { get; set; }
    public decimal Value { get; set; }
    public int CashRegisterId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
    public TransactionStatusRequest Status { get; set; } = TransactionStatusRequest.Pending;
}