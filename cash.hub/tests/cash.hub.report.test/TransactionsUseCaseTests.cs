using cash.hub.report.api.Adapters.Outbound.Cache;
using cash.hub.report.api.Application.Dto.Output;
using cash.hub.report.api.Application.UseCases;
using cash.hub.report.api.Domain.Ports;
using cash.hub.report.api.Domain.Entities;
using Moq;
using Moq.EntityFrameworkCore;

namespace cash.hub.report.test;

public class TransactionsUseCaseTests
{
   private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
    private readonly Mock<IRedisCacheService> _cacheMock;
    private readonly TransactionsUseCase _useCase;

    public TransactionsUseCaseTests()
    {
        _transactionRepositoryMock = new Mock<ITransactionRepository>();
        _cacheMock = new Mock<IRedisCacheService>();
        _useCase = new TransactionsUseCase(_transactionRepositoryMock.Object, _cacheMock.Object);
    }

    [Fact]
    public async Task GetTransactionsAsync_ShouldReturnCachedResult_WhenCacheIsAvailable()
    {
        // Arrange
        var date = DateTime.UtcNow;
        int pageNumber = 1, pageSize = 10;
        var cacheKey = $"transactions:{date:yyyyMMdd}:page{pageNumber}:size{pageSize}";
        var cachedResult = new PagedResult<Transaction>
        {
            Items = new List<Transaction> { new Transaction { Id = 1 } },
            TotalItems = 1,
            Page = pageNumber,
            PageSize = pageSize
        };

        _cacheMock.Setup(c => c.GetAsync<PagedResult<Transaction>>(cacheKey))
                  .ReturnsAsync(cachedResult);

        // Act
        var result = await _useCase.GetTransactionsAsync(date, pageNumber, pageSize);

        // Assert
        Assert.Equal(cachedResult, result);
        _transactionRepositoryMock.Verify(r => r.GetTransactionsByDate(It.IsAny<DateTime>()), Times.Never);
    }
}