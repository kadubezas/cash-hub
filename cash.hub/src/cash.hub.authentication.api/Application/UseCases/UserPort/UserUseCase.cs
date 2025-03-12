using System.Diagnostics;
using cash.hub.authentication.api.Application.Common.Dto;
using cash.hub.authentication.api.Application.Common.Enums;
using cash.hub.authentication.api.Application.Common.FactoryBaseReturn;
using cash.hub.authentication.api.Application.Services;
using cash.hub.authentication.api.Domain.Entities;
using cash.hub.authentication.api.Domain.Ports;

namespace cash.hub.authentication.api.Application.UseCases.UserPort;

public class UserUseCase (IUserRepository userRepository, 
                          ILogger<UserUseCase> logger,
                          IPasswordService passwordService,
                          IResponseFactory responseFactory) : IUserUseCase
{
    public async Task<BaseReturnApplication<bool>> RegisterAsync(string username, string password)
    {
        using var activity = Activity.Current?.Source.StartActivity();

        try
        {
            //Validar se existe alguém cadastrado com esse username
            var existUser = await userRepository.UserExistsAsync(username);

            if (existUser)
            {
                activity?.SetTag("USER_EXISTS", $"username {username} is already registered");
                logger.LogInformation("username is already registered");
                return responseFactory.Error<bool>(ErrorType.Business,$"username {username} is already registered");
            }
        
            //Gera o Salt
            var salt = passwordService.GenerateSalt();
        
            //Gera o hash da senha
            var passwordHash = passwordService.HashPassword(password, salt);
        
            //Salva o Usuário na base
            var user = new User
            {
                Username = username,
                PasswordHash = passwordHash,
                Salt = salt
            };
        
            await userRepository.AddUserAsync(user);
        
            return responseFactory.Success<bool>(true);
        }
        catch (Exception ex)
        {
            return responseFactory.Error<bool>(ErrorType.System, ex.Message);
        }
    }
}