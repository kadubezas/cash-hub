using System.ComponentModel.DataAnnotations;

namespace cash.hub.register.api.Domain.Entities;

public class CashRegister
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = String.Empty;

    public bool IsActive { get; set; } = true; // Indica se o caixa está ativo

    // Relacionamento: Um caixa pode ter várias transações
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}