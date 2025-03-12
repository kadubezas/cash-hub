using cash.hub.authentication.api.Application.Common;
using cash.hub.authentication.api.Application.Common.Dto;
using cash.hub.authentication.api.Application.Dto;

namespace cash.hub.authentication.api.Application.UseCases.TokenJwt;

public interface ITokenJwtUseCase
{
    public Task<BaseReturnApplication<TokenOut>> GenerateJwtTokenAsync(string userName, string password);
}