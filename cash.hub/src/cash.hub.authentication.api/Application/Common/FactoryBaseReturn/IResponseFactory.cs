using cash.hub.authentication.api.Application.Common.Dto;
using cash.hub.authentication.api.Application.Common.Enums;

namespace cash.hub.authentication.api.Application.Common.FactoryBaseReturn;

public interface IResponseFactory
{
    BaseReturnApplication<T> Success<T>(T data);
    BaseReturnApplication<T> Error<T>(ErrorType errorType, string message);
}