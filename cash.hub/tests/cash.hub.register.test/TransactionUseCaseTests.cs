using cash.hub.register.api.Application.Common.Dto;
using Moq;
using cash.hub.register.api.Application.Common.Enums;
using cash.hub.register.api.Application.Common.FactoryBaseReturn;
using cash.hub.register.api.Application.Dto.Inputs;
using cash.hub.register.api.Application.Dto.Outputs;
using cash.hub.register.api.Application.UseCases.Transactions;
using cash.hub.register.api.Domain.Entities;
using cash.hub.register.api.Domain.Entities.Enums;
using cash.hub.register.api.Domain.Ports;

namespace cash.hub.register.test;

public class TransactionUseCaseTests
{
    private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
    private readonly Mock<ICashRegisterRepository> _cashRegisterRepositoryMock;
    private readonly Mock<IResponseFactory> _responseFactoryMock;
    private readonly TransactionUseCase _transactionUseCase;

    public TransactionUseCaseTests()
    {
        _transactionRepositoryMock = new Mock<ITransactionRepository>();
        _cashRegisterRepositoryMock = new Mock<ICashRegisterRepository>();
        _responseFactoryMock = new Mock<IResponseFactory>();

        _transactionUseCase = new TransactionUseCase(
            _transactionRepositoryMock.Object,
            _cashRegisterRepositoryMock.Object,
            _responseFactoryMock.Object
        );
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnError_WhenCashRegisterDoesNotExist()
    {
        // Arrange
        var input = new TransactionInput { CashRegisterId = 1, Type = TransactionType.Credit, Value = 100 };

        _cashRegisterRepositoryMock
            .Setup(repo => repo.GetCashRegisterById(input.CashRegisterId))
            .ReturnsAsync((CashRegister?)null); // Simula caixa inexistente

        _responseFactoryMock
            .Setup(factory => factory.Error<TransactionOutput>(ErrorType.Business, "Caixa inexistente"))
            .Returns(new BaseReturnApplication<TransactionOutput>(false, null, ErrorType.Business, "Caixa inexistente"));

        // Act
        var result = await _transactionUseCase.RegisterAsync(input);

        // Assert
        Assert.Equal(ErrorType.Business, result.ErrorType);
        Assert.Equal("Caixa inexistente", result.Message);
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnError_WhenCashRegisterIsInactive()
    {
        // Arrange
        var input = new TransactionInput { CashRegisterId = 1, Type = TransactionType.Credit, Value = 100 };
        var cashRegister = new CashRegister { IsActive = false };

        _cashRegisterRepositoryMock
            .Setup(repo => repo.GetCashRegisterById(input.CashRegisterId))
            .ReturnsAsync(cashRegister);

        _responseFactoryMock
            .Setup(factory => factory.Error<TransactionOutput>(ErrorType.Business, "Caixa inativo"))
            .Returns(new BaseReturnApplication<TransactionOutput>(false, null, ErrorType.Business, "Caixa inativo"));

        // Act
        var result = await _transactionUseCase.RegisterAsync(input);

        // Assert
        Assert.Equal(ErrorType.Business, result.ErrorType);
        Assert.Equal("Caixa inativo", result.Message);
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnError_WhenInsufficientBalanceForDebit()
    {
        // Arrange
        var input = new TransactionInput { CashRegisterId = 1, Type = TransactionType.Debit, Value = 200 };
        var cashRegister = new CashRegister { IsActive = true, Balance = 100 };

        _cashRegisterRepositoryMock
            .Setup(repo => repo.GetCashRegisterById(input.CashRegisterId))
            .ReturnsAsync(cashRegister);

        _responseFactoryMock
            .Setup(factory => factory.Error<TransactionOutput>(ErrorType.Business, "Saldo insuficiente no caixa"))
            .Returns(new BaseReturnApplication<TransactionOutput>(false, null, ErrorType.Business, "Saldo insuficiente no caixa"));

        // Act
        var result = await _transactionUseCase.RegisterAsync(input);

        // Assert
        Assert.Equal(ErrorType.Business, result.ErrorType);
        Assert.Equal("Saldo insuficiente no caixa", result.Message);
    }

    [Fact]
    public async Task RegisterAsync_ShouldRegisterCreditTransaction_WhenValid()
    {
        // Arrange
        var input = new TransactionInput { CashRegisterId = 1, Type = TransactionType.Credit, Value = 500 };
        var cashRegister = new CashRegister { IsActive = true, Balance = 1000 };

        _cashRegisterRepositoryMock
            .Setup(repo => repo.GetCashRegisterById(input.CashRegisterId))
            .ReturnsAsync(cashRegister);

        _transactionRepositoryMock
            .Setup(repo => repo.AddTransactionAsync(It.IsAny<Transaction>()))
            .Returns(Task.CompletedTask);

        _cashRegisterRepositoryMock
            .Setup(repo => repo.ExecuteInTransactionAsync(It.IsAny<Func<Task>>()))
            .Returns(Task.CompletedTask);

        var output = new TransactionOutput()
        {
            TransactionId = Guid.NewGuid(),
            Status = TransactionStatus.Completed,
            CreatedAt = DateTime.Now
        };

        var expectedOutput = new BaseReturnApplication<TransactionOutput>(true, output, null, null);

        _responseFactoryMock
            .Setup(factory => factory.Success(It.IsAny<TransactionOutput>()))
            .Returns(expectedOutput);

        // Act
        var result = await _transactionUseCase.RegisterAsync(input);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Data);
        Assert.Equal(TransactionStatus.Completed, result.Data.Status);
    }

    [Fact]
    public async Task RegisterAsync_ShouldHandleException_AndReturnSystemError()
    {
        // Arrange
        var input = new TransactionInput { CashRegisterId = 1, Type = TransactionType.Credit, Value = 100 };

        _cashRegisterRepositoryMock
            .Setup(repo => repo.GetCashRegisterById(input.CashRegisterId))
            .ThrowsAsync(new Exception("Erro inesperado"));

        _responseFactoryMock
            .Setup(factory => factory.Error<TransactionOutput>(ErrorType.System, "Erro inesperado"))
            .Returns(new BaseReturnApplication<TransactionOutput>(false, null, ErrorType.System, "Erro inesperado"));

        // Act
        var result = await _transactionUseCase.RegisterAsync(input);

        // Assert
        Assert.Equal(ErrorType.System, result.ErrorType);
        Assert.Equal("Erro inesperado", result.Message);
    }
}