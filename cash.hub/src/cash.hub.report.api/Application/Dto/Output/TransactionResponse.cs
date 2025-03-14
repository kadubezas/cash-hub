namespace cash.hub.report.api.Application.Dto.Output;

public class TransactionResponse
{
    public Guid TransactionId { get; set; }
    public string Type { get; set; }
    public decimal Value { get; set; }
    public string CreatedAt { get; set; } // Melhor retornar string formatada
    public string Status { get; set; }
}