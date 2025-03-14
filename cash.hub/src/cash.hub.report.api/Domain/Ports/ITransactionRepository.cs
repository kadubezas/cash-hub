using cash.hub.report.api.Application.Dto.Output;
using cash.hub.report.api.Domain.Entities;

namespace cash.hub.report.api.Domain.Ports;

public interface ITransactionRepository
{
    IQueryable<Transaction> GetTransactionsByDate(DateTime date);
}