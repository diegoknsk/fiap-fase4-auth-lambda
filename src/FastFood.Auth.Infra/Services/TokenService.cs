using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using FastFood.Auth.Application.Ports;

namespace FastFood.Auth.Infra.Services;

/// <summary>
/// Implementação do serviço de geração de tokens JWT.
/// </summary>
public class TokenService(IConfiguration configuration) : ITokenService
{
    public string GenerateToken(Guid customerId, out DateTime expiresAt)
    {
        // JWT Secret deve vir EXCLUSIVAMENTE de variável de ambiente por segurança
        var secret = Environment.GetEnvironmentVariable("JwtSettings_Secret")
            ?? throw new InvalidOperationException("JWT Secret não configurado. Configure a variável de ambiente 'JwtSettings_Secret' (mínimo 32 caracteres).");
        
        if (secret.Length < 32)
        {
            throw new InvalidOperationException($"JWT Secret deve ter no mínimo 32 caracteres. Valor atual tem {secret.Length} caracteres.");
        }

        var jwtSettings = configuration.GetSection("JwtSettings");
        var issuer = jwtSettings["Issuer"] ?? throw new InvalidOperationException("JWT Issuer não configurado");
        var audience = jwtSettings["Audience"] ?? throw new InvalidOperationException("JWT Audience não configurado");
        var expirationHours = int.Parse(jwtSettings["ExpirationHours"] ?? "24");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var now = DateTime.UtcNow;
        expiresAt = now.AddHours(expirationHours);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, customerId.ToString()),
            new Claim("customerId", customerId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiresAt,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}





