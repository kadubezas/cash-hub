using cash.hub.register.api.Application.Common.Dto;
using cash.hub.register.api.Application.Common.Enums;
using cash.hub.register.api.Application.Common.FactoryBaseReturn;
using cash.hub.register.api.Application.Dto.Inputs;
using cash.hub.register.api.Application.Dto.Outputs;
using cash.hub.register.api.Domain.Entities;
using cash.hub.register.api.Domain.Entities.Enums;
using cash.hub.register.api.Domain.Ports;

namespace cash.hub.register.api.Application.UseCases.Transactions;

public class TransactionUseCase(ITransactionRepository transactionRepository, 
                                ICashRegisterRepository cashRegisterRepository,
                                IResponseFactory responseFactory) : ITransactionUseCase
{
    public async Task<BaseReturnApplication<TransactionOutput>> RegisterAsync(TransactionInput input)
    {
        try
        {
            // Consulta o caixa
            var cashRegister = await cashRegisterRepository.GetCashRegisterById(input.CashRegisterId);

            // Valida se o caixa existe
            if (cashRegister == null)
            {
                return responseFactory.Error<TransactionOutput>(ErrorType.Business, "Caixa inexistente");
            }

            // Valida se o caixa está ativo
            if (!cashRegister.IsActive)
            {
                return responseFactory.Error<TransactionOutput>(ErrorType.Business, "Caixa inativo");
            }

            // Valida saldo apenas para Débitos
            if (input.Type == TransactionType.Debit && cashRegister.Balance < input.Value)
            {
                return responseFactory.Error<TransactionOutput>(ErrorType.Business, "Saldo insuficiente no caixa");
            }

            // Criar a transação validada
            var transaction = new Transaction()
            {
                Type = input.Type,
                CashRegisterId = input.CashRegisterId,
                Status = TransactionStatus.Completed,
                Value = input.Value
            };

            // Define ajuste de saldo baseado no tipo de transação
            decimal saldoAjustado = input.Type switch
            {
                TransactionType.Credit => input.Value,  
                TransactionType.Debit => -input.Value,  
                TransactionType.Refund => input.Value,  
                _ => 0
            };
            
            // Executa as operações dentro da transação do repositório
            await cashRegisterRepository.ExecuteInTransactionAsync(async () =>
            {
                await cashRegisterRepository.UpdateBalanceAsync(transaction.CashRegisterId, saldoAjustado);
                await transactionRepository.AddTransactionAsync(transaction);
            });

            // Criar o objeto de output
            var output = new TransactionOutput()
            {
                TransactionId = transaction.TransactionId,
                Status = transaction.Status,
                CreatedAt = transaction.CreatedAt
            };

            //Retorna para o Endpoint
            return responseFactory.Success(output);
        }
        catch (Exception ex)
        {
            return responseFactory.Error<TransactionOutput>(ErrorType.System, ex.Message);
        }
    }
}