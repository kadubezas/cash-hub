using cash.hub.register.api.Domain.Entities.Enums;

namespace cash.hub.register.api.Application.Dto.Inputs;

public record TransactionInput
{
    public TransactionType Type { get; set; }
    public decimal Value { get; set; }
    public int CashRegisterId { get; set; }
    public string? Notes { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public int? Installments { get; set; }
}