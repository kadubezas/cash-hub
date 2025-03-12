using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using cash.hub.authentication.api.Application.Common.Config;
using cash.hub.authentication.api.Application.Services;
using FluentAssertions;
using Microsoft.Extensions.Options;

namespace cash.hub.authentication.test;

public class GenerateTokenServiceTests
{
    private readonly GenerateTokenService _generateTokenService;

    public GenerateTokenServiceTests()
    {
        // Simulando as configurações do JWT
        var jwtSettingsMock = Options.Create(new JwtSettings
        {
            Secret = "MinhaChaveSuperSegura1234567890123456", // Deve ter pelo menos 32 caracteres
            Issuer = "cash.hub.authentication.api",
            Audience = "cash.hub.clients",
            ExpirationInMinutes = 60
        });

        // Criando a instância do serviço com configurações mockadas
        _generateTokenService = new GenerateTokenService(jwtSettingsMock);
    }

    [Fact]
    public void GenerateToken_ShouldReturn_ValidJwtToken()
    {
        // Arrange
        string userId = "123";
        string userName = "testuser";

        // Act
        var token = _generateTokenService.GenerateToken(userId, userName, out DateTime expires);

        // Assert
        token.Should().NotBeNullOrWhiteSpace();
        expires.Should().BeAfter(DateTime.UtcNow);

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
        
        jwtToken.Should().NotBeNull();
        jwtToken.Issuer.Should().Be("cash.hub.authentication.api");
        jwtToken.Audiences.Should().Contain("cash.hub.clients");
        
        var claims = jwtToken.Claims.ToList();
        claims.Should().Contain(c => c.Type == "unique_name" && c.Value == userName);
        claims.Should().Contain(c => c.Type == "UserId" && c.Value == userId);
    }

    [Fact]
    public void GenerateToken_ShouldSet_CorrectExpirationTime()
    {
        // Arrange
        string userId = "123";
        string userName = "testuser";

        // Act
        var token = _generateTokenService.GenerateToken(userId, userName, out DateTime expires);

        // Assert
        expires.Should().BeCloseTo(DateTime.UtcNow.AddMinutes(60), TimeSpan.FromSeconds(5)); // Margem de erro de 5 segundos
    }
}