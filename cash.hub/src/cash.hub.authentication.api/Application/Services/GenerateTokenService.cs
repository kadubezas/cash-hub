using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using cash.hub.authentication.api.Application.Common.Config;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace cash.hub.authentication.api.Application.Services;

public class GenerateTokenService(IOptions<JwtSettings> jwtSettings) : IGenerateTokenService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;
    
    public string GenerateToken(string userId, string userName, out DateTime expires)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);
        expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim("UserId", userId)
            }),
            Expires = expires,
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        
        return jwtToken;
    }
}