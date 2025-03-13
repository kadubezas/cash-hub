using cash.hub.register.api.Domain.Entities;
using cash.hub.register.api.Domain.Ports;

namespace cash.hub.register.api.Adapters.Outbound.Repository;

public class CashRegisterRepository : ICashRegisterRepository
{
    public Task<CashRegister?> GetCashRegisterById(int id)
    {
        throw new NotImplementedException();
    }
}