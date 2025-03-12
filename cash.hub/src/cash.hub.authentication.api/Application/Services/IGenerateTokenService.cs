namespace cash.hub.authentication.api.Application.Services;

public interface IGenerateTokenService
{
    public string GenerateToken(string userId, string userName, out DateTime expires);
}