using System.Validation;
using cash.hub.register.api.Adapters.Inbound.Rest.Common.Enums;
using cash.hub.register.api.Adapters.Inbound.Rest.Requests;

namespace cash.hub.register.api.Adapters.Inbound.Rest.Validators;

public class RegisterTransactionValidator : FlatValidator<RegisterTransactionRequest>
{
    public RegisterTransactionValidator()
    {
        // Valor deve ser maior que zero
        ErrorIf(m => m.Value <= 0, "O valor da transação deve ser maior que zero.", m => m.Value);

        // ID do caixa deve ser válido
        ErrorIf(m => m.CashRegisterId <= 0, "O ID do caixa deve ser maior que zero.", m => m.CashRegisterId);

        // Parcelas devem ser informadas e > 0 se o método de pagamento for crédito
        ValidIf(
            m => m.PaymentMethod != PaymentMethodRequest.CreditCard || (m.Installments.HasValue && m.Installments > 0),
            "Para pagamentos com crédito, o número de parcelas deve ser informado e maior que zero.",
            m => m.Installments
        );

        // Parcelas NÃO devem ser informadas se o método de pagamento NÃO for crédito
        ErrorIf(
            m => m.PaymentMethod != PaymentMethodRequest.CreditCard && m.Installments.HasValue,
            "Parcelas só devem ser informadas para pagamentos com crédito.",
            m => m.Installments
        );
    }
}