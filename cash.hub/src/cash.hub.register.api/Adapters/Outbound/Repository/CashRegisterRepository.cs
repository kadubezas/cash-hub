using cash.hub.register.api.Adapters.Outbound.Repository.DataBaseContextConfiguration;
using cash.hub.register.api.Domain.Entities;
using cash.hub.register.api.Domain.Ports;
using Microsoft.EntityFrameworkCore;

namespace cash.hub.register.api.Adapters.Outbound.Repository;

public class CashRegisterRepository(DataBaseContext context) : ICashRegisterRepository
{
    public async Task<CashRegister?> GetCashRegisterById(int id)
    {
        return await context.CashRegisters.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task UpdateBalanceAsync(int cashRegisterId, decimal value)
    {
        var cashRegister = await GetCashRegisterById(cashRegisterId);
        if (cashRegister != null)
        {
            cashRegister.Balance += value;
            context.CashRegisters.Update(cashRegister);
            await context.SaveChangesAsync();
        }
    }

    public async Task ExecuteInTransactionAsync(Func<Task> operation)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await operation(); 
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}