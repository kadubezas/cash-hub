using cash.hub.register.api.Adapters.Outbound.Repository.DataBaseContextConfiguration;
using cash.hub.register.api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace cash.hub.register.api.infra.EntityFramework;

public static class DbInitializer
{
    public static void Seed(DataBaseContext context)
    {
        context.Database.Migrate();
        
        if (context.CashRegisters.Any())
        {
            return;
        }

        var cashRegisters = new List<CashRegister>
        {
            new CashRegister { Name = "Caixa 01", IsActive = true },
            new CashRegister { Name = "Caixa 02", IsActive = true },
            new CashRegister { Name = "Caixa 03", IsActive = false },
            new CashRegister { Name = "Caixa 04", IsActive = true }
        };

        context.CashRegisters.AddRange(cashRegisters);
        context.SaveChanges();
    }
}