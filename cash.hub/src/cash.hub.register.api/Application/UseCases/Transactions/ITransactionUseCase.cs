using cash.hub.register.api.Adapters.Inbound.Rest.Requests;
using cash.hub.register.api.Application.Common.Dto;
using cash.hub.register.api.Application.Dto;
using cash.hub.register.api.Application.Dto.Inputs;
using cash.hub.register.api.Application.Dto.Outputs;

namespace cash.hub.register.api.Application.UseCases.Transactions;

public interface ITransactionUseCase
{
    Task<BaseReturnApplication<TransactionOutput>> RegisterAsync(TransactionInput input);
}