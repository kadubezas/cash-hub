using FluentAssertions;
using cash.hub.authentication.api.Application.Services;

namespace cash.hub.authentication.test;

public class PasswordServiceTests
{
    private readonly PasswordService _passwordService;

    public PasswordServiceTests()
    {
        _passwordService = new PasswordService();
    }

    [Fact]
    public void GenerateSalt_ShouldReturnBase64String_WithCorrectLength()
    {
        // Act
        string salt = _passwordService.GenerateSalt();

        // Assert
        salt.Should().NotBeNullOrWhiteSpace();
        byte[] saltBytes = Convert.FromBase64String(salt);
        saltBytes.Length.Should().Be(16, "o salt deve ter exatamente 16 bytes");
    }

    [Fact]
    public void HashPassword_ShouldReturnConsistentHash_WhenSameInputIsProvided()
    {
        // Arrange
        string password = "MySecurePassword";
        string salt = _passwordService.GenerateSalt();

        // Act
        string hash1 = _passwordService.HashPassword(password, salt);
        string hash2 = _passwordService.HashPassword(password, salt);

        // Assert
        hash1.Should().NotBeNullOrWhiteSpace();
        hash1.Should().Be(hash2, "a mesma senha e o mesmo salt devem gerar hashes idênticos");
    }

    [Fact]
    public void HashPassword_ShouldReturnDifferentHashes_ForDifferentSalts()
    {
        // Arrange
        string password = "MySecurePassword";
        string salt1 = _passwordService.GenerateSalt();
        string salt2 = _passwordService.GenerateSalt();

        // Act
        string hash1 = _passwordService.HashPassword(password, salt1);
        string hash2 = _passwordService.HashPassword(password, salt2);

        // Assert
        hash1.Should().NotBe(hash2, "senhas com diferentes salts devem gerar hashes diferentes");
    }

    [Fact]
    public void HashPassword_ShouldReturnDifferentHashes_ForDifferentPasswords()
    {
        // Arrange
        string salt = _passwordService.GenerateSalt();
        string password1 = "Password1";
        string password2 = "Password2";

        // Act
        string hash1 = _passwordService.HashPassword(password1, salt);
        string hash2 = _passwordService.HashPassword(password2, salt);

        // Assert
        hash1.Should().NotBe(hash2, "senhas diferentes devem gerar hashes diferentes mesmo com o mesmo salt");
    }
}