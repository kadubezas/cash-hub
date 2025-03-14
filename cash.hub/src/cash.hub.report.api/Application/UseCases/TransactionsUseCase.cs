using cash.hub.report.api.Adapters.Outbound.Cache;
using cash.hub.report.api.Application.Dto.Output;
using cash.hub.report.api.Domain.Entities;
using cash.hub.report.api.Domain.Ports;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace cash.hub.report.api.Application.UseCases;

public class TransactionsUseCase : ITransactionUseCase
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IRedisCacheService _cache;
    private readonly TimeSpan _cacheExpiration;

    public TransactionsUseCase(ITransactionRepository transactionRepository, IRedisCacheService cache)
    {
        _transactionRepository = transactionRepository;
        _cache = cache;
        _cacheExpiration = TimeSpan.FromMinutes(5);
    }
    public async Task<PagedResult<Transaction>> GetTransactionsAsync(DateTime date, int pageNumber, int pageSize)
    {
        string cacheKey = $"transactions:{date:yyyyMMdd}:page{pageNumber}:size{pageSize}";

        // 1️⃣ Tenta buscar do cache antes de acessar o banco
        var cachedResult = await _cache.GetAsync<PagedResult<Transaction>>(cacheKey);
        if (cachedResult != null)
        {
            return cachedResult;
        }

        // 2️⃣ Executa a consulta (Agora `ToListAsync()` acontece no UseCase)
        var query = _transactionRepository.GetTransactionsByDate(date);
        var totalItems = await query.CountAsync();
        var transactions = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(); // Aqui executamos a query

        var result = new PagedResult<Transaction>
        {
            Items = transactions,
            TotalItems = totalItems,
            Page = pageNumber,
            PageSize = pageSize
        };

        // 3️⃣ Armazena no cache
        await _cache.SetAsync(cacheKey, result, _cacheExpiration);

        return result;
    }
}