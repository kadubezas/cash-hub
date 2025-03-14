using cash.hub.report.api.Adapters.Outbound.Repository.DataBaseContextConfiguration;
using cash.hub.report.api.Application.Dto.Output;
using cash.hub.report.api.Domain.Entities;
using cash.hub.report.api.Domain.Ports;
using Microsoft.EntityFrameworkCore;

namespace cash.hub.report.api.Adapters.Outbound.Repository;

public class TransactionRepository(DataBaseContext context) : ITransactionRepository
{
    public IQueryable<Transaction> GetTransactionsByDate(DateTime date)
    {
        var startDate = date.Date;
        var endDate = startDate.AddDays(1);

        var query = context.Transactions
            .Where(t => t.CreatedAt >= startDate && t.CreatedAt < endDate)
            .OrderByDescending(t => t.CreatedAt);

        return query;
    }
}