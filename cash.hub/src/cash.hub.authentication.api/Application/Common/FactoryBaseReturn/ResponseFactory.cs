using cash.hub.authentication.api.Application.Common.Dto;
using cash.hub.authentication.api.Application.Common.Enums;

namespace cash.hub.authentication.api.Application.Common.FactoryBaseReturn;

public class ResponseFactory : IResponseFactory
{
    public BaseReturnApplication<T> Success<T>(T data)
    {
        return new BaseReturnApplication<T>(true, data, null, "Operação realizada com sucesso");
    }

    public BaseReturnApplication<T> Error<T>(ErrorType errorType, string message)
    {
        return new BaseReturnApplication<T>(false, default, errorType, message);
    }
}