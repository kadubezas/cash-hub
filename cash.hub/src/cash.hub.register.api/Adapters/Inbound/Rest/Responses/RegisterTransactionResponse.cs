using cash.hub.register.api.Adapters.Inbound.Rest.Common.Enums;

namespace cash.hub.register.api.Adapters.Inbound.Rest.Responses;

public record RegisterTransactionResponse
{
    public Guid TransactionId { get; set; }
    public TransactionStatusRequest Status { get; set; }
    public DateTime CreatedAt { get; set; }
}