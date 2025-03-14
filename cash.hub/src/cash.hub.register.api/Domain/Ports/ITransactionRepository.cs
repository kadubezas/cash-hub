using cash.hub.register.api.Domain.Entities;

namespace cash.hub.register.api.Domain.Ports;

public interface ITransactionRepository
{
    Task<Transaction?> GetTransactionByTransactionIdAsync(Guid transactionId);
    Task AddTransactionAsync(Transaction transaction);
}