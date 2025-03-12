using cash.hub.register.api.Domain.Entities.Enums;

namespace cash.hub.register.api.Domain.Entities;

public class Transaction
{
    public Guid TransactionIdentification { get; set; }
    public TransactionType Type  { get; set; }
    public decimal Value { get; set; }
}