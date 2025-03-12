using System.Security.Cryptography;
using System.Text;

namespace cash.hub.authentication.api.Application.Services;

public class PasswordService : IPasswordService
{
    public string GenerateSalt()
    {
        byte[] saltBytes = new byte[16];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(saltBytes);
        return Convert.ToBase64String(saltBytes);
    }

    public string HashPassword(string password, string salt)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password + salt);
        return Convert.ToBase64String(sha256.ComputeHash(bytes));
    }
}