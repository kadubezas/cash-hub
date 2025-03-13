using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using cash.hub.register.api.Domain.Entities.Enums;

namespace cash.hub.register.api.Domain.Entities;

public class Transaction
{
    [Key]
    public int Id { get; set; }
    
    public Guid TransactionId { get; set; } = Guid.NewGuid(); 
    
    [Required]
    public TransactionType Type { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Value { get; set; }
    
    [Required]
    public int CashRegisterId { get; set; }
    
    [ForeignKey("CashRegisterId")]
    public virtual CashRegister CashRegister { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
    
    [Required]
    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;

    // Relacionamento com a transação estornada
    public int? RefundedTransactionId { get; set; }
    public virtual Transaction? RefundedTransaction { get; set; }
}