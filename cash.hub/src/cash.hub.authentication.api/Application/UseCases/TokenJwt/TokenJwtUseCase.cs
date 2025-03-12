using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using cash.hub.authentication.api.Application.Common;
using cash.hub.authentication.api.Application.Common.Config;
using cash.hub.authentication.api.Application.Common.Dto;
using cash.hub.authentication.api.Application.Common.Enums;
using cash.hub.authentication.api.Application.Common.FactoryBaseReturn;
using cash.hub.authentication.api.Application.Dto;
using cash.hub.authentication.api.Application.Services;
using cash.hub.authentication.api.Domain.Ports;

namespace cash.hub.authentication.api.Application.UseCases.TokenJwt;

public class TokenJwtUseCase(IUserRepository userRepository,
                             IResponseFactory responseFactory,
                             IPasswordService passwordService,
                             IGenerateTokenService generateTokenService) : ITokenJwtUseCase
{
    public async Task<BaseReturnApplication<TokenOut>> GenerateJwtTokenAsync(string userName, string password)
    {
        try
        {
            // Verifica se o usuário existe no banco
            var user = await userRepository.GetByUsernameAsync(userName);
        
            if (user == null) return responseFactory.Error<TokenOut>(ErrorType.Business, "Usuário ou senha inválidos");
        
            // Valida a senha
            var hash = passwordService.HashPassword(password, user.Salt);
            if (hash != user.PasswordHash)
            {
                return responseFactory.Error<TokenOut>(ErrorType.Business, "Usuário ou senha inválidos");
            }

            // Gerando Token JWT
            var jwtToken = generateTokenService.GenerateToken(user.Id.ToString(), user.Username, out var expires);

            return responseFactory.Success(new TokenOut(jwtToken, expires));
        }
        catch (Exception ex)
        {
            return responseFactory.Error<TokenOut>(ErrorType.System, ex.Message);
        }
    }
}