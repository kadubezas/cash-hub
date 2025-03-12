namespace cash.hub.authentication.api.Application.Services;

public interface IPasswordService
{
    public string GenerateSalt();

    public string HashPassword(string password, string salt);
}