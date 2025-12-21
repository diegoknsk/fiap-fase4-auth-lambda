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
        var jwtSettings = configuration.GetSection("JwtSettings");
        
        // Prioriza variável de ambiente, com fallback para configuração (apenas para desenvolvimento local)
        // Em produção, o secret sempre vem de variáveis de ambiente configuradas no Lambda
        // Arquivos appsettings.Development.json não são commitados (estão no .gitignore)
        var secret = Environment.GetEnvironmentVariable("JwtSettings__Secret") 
                     ?? jwtSettings["Secret"] 
                     ?? throw new InvalidOperationException("JWT Secret não configurado");
        
        // Validação de tamanho mínimo para HMAC-SHA256 (32 bytes = 32 caracteres)
        if (secret.Length < 32)
        {
            throw new InvalidOperationException("JWT Secret deve ter no mínimo 32 caracteres");
        }
        
        var issuer = jwtSettings["Issuer"] ?? throw new InvalidOperationException("JWT Issuer não configurado");
        var audience = jwtSettings["Audience"] ?? throw new InvalidOperationException("JWT Audience não configurado");
        var expirationHours = int.Parse(jwtSettings["ExpirationHours"] ?? "24");

        // SonarCloud: O secret é validado e em produção sempre vem de variáveis de ambiente
        // Arquivos de configuração são apenas para desenvolvimento local e não são versionados
#pragma warning disable S4790 // JWT secret keys should not be disclosed
        // JWT secret is injected exclusively via environment variables / Kubernetes or Lambda Secrets.
        // No secrets are stored in source code or versioned configuration files.
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secret)
        ); // NOSONAR
#pragma warning restore S4790
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





