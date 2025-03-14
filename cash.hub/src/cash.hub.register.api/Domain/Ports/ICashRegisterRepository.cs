using cash.hub.register.api.Domain.Entities;

namespace cash.hub.register.api.Domain.Ports;

public interface ICashRegisterRepository
{
    Task<CashRegister?> GetCashRegisterById(int id);
    Task UpdateBalanceAsync(int cashRegisterId, decimal value);
    Task ExecuteInTransactionAsync(Func<Task> operation);
}