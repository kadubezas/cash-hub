using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cash.hub.register.api.Domain.Entities;

public class CashRegister
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = String.Empty;

    public bool IsActive { get; set; } = true;
    
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Balance { get; set; }

    // Relacionamento: Um caixa pode ter várias transações
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}