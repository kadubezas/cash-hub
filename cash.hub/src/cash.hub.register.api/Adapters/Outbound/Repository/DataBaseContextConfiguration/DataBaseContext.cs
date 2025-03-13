using cash.hub.register.api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace cash.hub.register.api.Adapters.Outbound.Repository.DataBaseContextConfiguration;

public class DataBaseContext : DbContext
{
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<CashRegister> CashRegisters { get; set; }

    public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Relacionamento entre Transaction e CashRegister
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.CashRegister)
            .WithMany(c => c.Transactions)
            .HasForeignKey(t => t.CashRegisterId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.RefundedTransaction)
            .WithMany()
            .HasForeignKey(t => t.RefundedTransactionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}