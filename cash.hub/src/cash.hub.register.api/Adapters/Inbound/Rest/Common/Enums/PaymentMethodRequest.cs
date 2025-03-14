namespace cash.hub.register.api.Adapters.Inbound.Rest.Common.Enums;

public enum PaymentMethodRequest
{
    Cash,       // Dinheiro
    CreditCard, // Cartão de crédito
    DebitCard,  // Cartão de débito
    Pix,        // PIX
    Voucher,    // Vale-alimentação ou refeição
    Other  
}