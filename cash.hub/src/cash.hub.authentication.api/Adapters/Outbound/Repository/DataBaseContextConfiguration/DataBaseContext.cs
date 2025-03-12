using cash.hub.authentication.api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace cash.hub.authentication.api.Adapters.Outbound.Repository.DataBaseContextConfiguration;

public class DataBaseContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .ToTable("Users");
        
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
    }
}