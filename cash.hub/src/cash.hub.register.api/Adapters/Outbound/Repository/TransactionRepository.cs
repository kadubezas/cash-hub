using cash.hub.register.api.Domain.Entities;
using cash.hub.register.api.Domain.Ports;

namespace cash.hub.register.api.Adapters.Outbound.Repository;

public class TransactionRepository  : ITransactionRepository
{
    public Task<Transaction> GetTransactionByTransactionIdAsync(string transactionId)
    {
        throw new NotImplementedException();
    }

    public Task AddTransactionAsync(Transaction transaction)
    {
        throw new NotImplementedException();
    }
}