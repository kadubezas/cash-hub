using cash.hub.register.api.Domain.Entities.Enums;

namespace cash.hub.register.api.Application.Dto.Outputs;

public class TransactionOutput
{
    public Guid TransactionId { get; set; }
    public TransactionStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}