using cash.hub.report.api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace cash.hub.report.api.Adapters.Outbound.Repository.DataBaseContextConfiguration;

public class DataBaseContext : DbContext
{
    public DbSet<Transaction> Transactions { get; set; }

    public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Define a chave primária
        modelBuilder.Entity<Transaction>()
            .HasKey(t => t.Id);
        
        base.OnModelCreating(modelBuilder);
    }
}