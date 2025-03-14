using cash.hub.register.api.Adapters.Outbound.Repository.DataBaseContextConfiguration;
using cash.hub.register.api.Domain.Entities;
using cash.hub.register.api.Domain.Ports;
using Microsoft.EntityFrameworkCore;

namespace cash.hub.register.api.Adapters.Outbound.Repository;

public class TransactionRepository(DataBaseContext context, ILogger<TransactionRepository> logger)  : ITransactionRepository
{
    public async Task<Transaction?> GetTransactionByTransactionIdAsync(Guid transactionId)
    {
        return await context.Transactions.FirstOrDefaultAsync(t => t.TransactionId == transactionId); 
    }

    public async Task AddTransactionAsync(Transaction transaction)
    {
        context.Transactions.Add(transaction);
        await context.SaveChangesAsync();
    }
}