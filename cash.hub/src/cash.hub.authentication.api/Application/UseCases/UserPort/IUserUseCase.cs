using cash.hub.authentication.api.Application.Common;
using cash.hub.authentication.api.Application.Common.Dto;

namespace cash.hub.authentication.api.Application.UseCases.UserPort;

public interface IUserUseCase
{
    public Task<BaseReturnApplication<bool>> RegisterAsync(string username, string password);
}