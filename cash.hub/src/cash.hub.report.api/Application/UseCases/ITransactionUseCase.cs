using cash.hub.report.api.Application.Dto.Output;
using cash.hub.report.api.Domain.Entities;

namespace cash.hub.report.api.Application.UseCases;

public interface ITransactionUseCase
{
    Task<PagedResult<Transaction>> GetTransactionsAsync(DateTime date, int pageNumber, int pageSize);
}