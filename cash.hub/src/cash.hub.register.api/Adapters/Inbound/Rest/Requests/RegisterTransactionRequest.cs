using cash.hub.register.api.Adapters.Inbound.Rest.Common.Enums;
using cash.hub.register.api.Domain.Entities.Enums;

namespace cash.hub.register.api.Adapters.Inbound.Rest.Requests;

public record RegisterTransactionRequest
{
    public TransactionTypeRequest Type { get; set; }
    public decimal Value { get; set; }
    public int CashRegisterId { get; set; }
    public string? Notes { get; set; } //Detalhes extras sobre a transação
    public PaymentMethodRequest PaymentMethod { get; set; }
    public int? Installments { get; set; } //Se o método de pagamento for crédito
}