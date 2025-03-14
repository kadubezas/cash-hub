using cash.hub.report.api.Domain.Entities.Enums;

namespace cash.hub.report.api.Domain.Entities;

public class Transaction
{
    public int Id { get; set; }
    public Guid TransactionId { get; set; } = Guid.NewGuid(); 
    public TransactionType Type { get; set; }
    public decimal Value { get; set; }
    public int CashRegisterId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;
    public int? RefundedTransactionId { get; set; }
}