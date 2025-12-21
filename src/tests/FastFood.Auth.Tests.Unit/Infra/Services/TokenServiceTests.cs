using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FastFood.Auth.Infra.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace FastFood.Auth.Tests.Unit.Infra.Services;

/// <summary>
/// Testes unitários para TokenService.
/// </summary>
public class TokenServiceTests : IDisposable
{
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<IConfigurationSection> _jwtSettingsSectionMock;
    private readonly TokenService _tokenService;
    private const string TestSecret = "MySuperSecretKeyThatIsAtLeast32CharactersLong";

    public TokenServiceTests()
    {
        // Configurar variável de ambiente para o secret (obrigatório agora)
        Environment.SetEnvironmentVariable("JwtSettings_Secret", TestSecret);

        _configurationMock = new Mock<IConfiguration>();
        _jwtSettingsSectionMock = new Mock<IConfigurationSection>();

        // Setup default configuration (apenas Issuer, Audience, ExpirationHours - Secret vem de ENV)
        _jwtSettingsSectionMock.Setup(x => x["Issuer"])
            .Returns("FastFood.Auth");
        _jwtSettingsSectionMock.Setup(x => x["Audience"])
            .Returns("FastFood.API");
        _jwtSettingsSectionMock.Setup(x => x["ExpirationHours"])
            .Returns("24");

        _configurationMock.Setup(x => x.GetSection("JwtSettings"))
            .Returns(_jwtSettingsSectionMock.Object);

        _tokenService = new TokenService(_configurationMock.Object);
    }

    public void Dispose()
    {
        // Limpar variável de ambiente após os testes
        Environment.SetEnvironmentVariable("JwtSettings_Secret", null);
    }

    [Fact]
    public void GenerateToken_WithValidConfiguration_ShouldReturnValidToken()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        // Act
        var token = _tokenService.GenerateToken(customerId, out var expiresAt);

        // Assert
        Assert.NotNull(token);
        Assert.NotEmpty(token);
        Assert.True(expiresAt > DateTime.UtcNow);
    }

    [Fact]
    public void GenerateToken_ShouldSetCorrectExpiresAt()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var beforeGeneration = DateTime.UtcNow;

        // Act
        _tokenService.GenerateToken(customerId, out var expiresAt);

        // Assert
        var afterGeneration = DateTime.UtcNow;
        var expectedMinExpiration = beforeGeneration.AddHours(24);
        var expectedMaxExpiration = afterGeneration.AddHours(24).AddSeconds(5);
        Assert.True(expiresAt >= expectedMinExpiration && expiresAt <= expectedMaxExpiration);
    }

    [Fact]
    public void GenerateToken_ShouldIncludeCustomerIdInClaims()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        // Act
        var token = _tokenService.GenerateToken(customerId, out _);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);
        var customerIdClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == "customerId");

        Assert.NotNull(customerIdClaim);
        Assert.Equal(customerId.ToString(), customerIdClaim.Value);
    }

    [Fact]
    public void GenerateToken_ShouldIncludeJtiClaim()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        // Act
        var token = _tokenService.GenerateToken(customerId, out _);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);
        var jtiClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti);

        Assert.NotNull(jtiClaim);
        Assert.True(Guid.TryParse(jtiClaim.Value, out _));
    }

    [Fact]
    public void GenerateToken_ShouldIncludeIatClaim()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        // Act
        var token = _tokenService.GenerateToken(customerId, out _);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);
        var iatClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Iat);

        Assert.NotNull(iatClaim);
        Assert.True(long.TryParse(iatClaim.Value, out var iatValue));
        var iatDateTime = DateTimeOffset.FromUnixTimeSeconds(iatValue).DateTime;
        Assert.True(Math.Abs((DateTime.UtcNow - iatDateTime).TotalSeconds) < 5);
    }

    [Fact]
    public void GenerateToken_ShouldIncludeSubClaim()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        // Act
        var token = _tokenService.GenerateToken(customerId, out _);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);
        var subClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);

        Assert.NotNull(subClaim);
        Assert.Equal(customerId.ToString(), subClaim.Value);
    }

    [Fact]
    public void GenerateToken_ShouldHaveCorrectIssuer()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        // Act
        var token = _tokenService.GenerateToken(customerId, out _);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);

        Assert.Equal("FastFood.Auth", jsonToken.Issuer);
    }

    [Fact]
    public void GenerateToken_ShouldHaveCorrectAudience()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        // Act
        var token = _tokenService.GenerateToken(customerId, out _);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);

        Assert.Contains("FastFood.API", jsonToken.Audiences);
    }

    [Fact]
    public void GenerateToken_ShouldUseHmacSha256Algorithm()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        // Act
        var token = _tokenService.GenerateToken(customerId, out _);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);

        Assert.Equal("HS256", jsonToken.SignatureAlgorithm);
    }

    [Fact]
    public void GenerateToken_WithMissingSecret_ShouldThrowInvalidOperationException()
    {
        // Arrange
        Environment.SetEnvironmentVariable("JwtSettings_Secret", null);
        var tokenService = new TokenService(_configurationMock.Object);
        var customerId = Guid.NewGuid();

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            tokenService.GenerateToken(customerId, out _));
        Assert.Contains("JWT Secret não configurado", exception.Message);
        
        // Restaurar para outros testes
        Environment.SetEnvironmentVariable("JwtSettings_Secret", TestSecret);
    }

    [Fact]
    public void GenerateToken_WithSecretTooShort_ShouldThrowInvalidOperationException()
    {
        // Arrange
        Environment.SetEnvironmentVariable("JwtSettings_Secret", "short");
        var tokenService = new TokenService(_configurationMock.Object);
        var customerId = Guid.NewGuid();

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            tokenService.GenerateToken(customerId, out _));
        Assert.Contains("mínimo 32 caracteres", exception.Message);
        
        // Restaurar para outros testes
        Environment.SetEnvironmentVariable("JwtSettings_Secret", TestSecret);
    }

    [Fact]
    public void GenerateToken_WithMissingIssuer_ShouldThrowInvalidOperationException()
    {
        // Arrange
        _jwtSettingsSectionMock.Setup(x => x["Issuer"]).Returns((string?)null);
        var tokenService = new TokenService(_configurationMock.Object);
        var customerId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            tokenService.GenerateToken(customerId, out _));
    }

    [Fact]
    public void GenerateToken_WithMissingAudience_ShouldThrowInvalidOperationException()
    {
        // Arrange
        _jwtSettingsSectionMock.Setup(x => x["Audience"]).Returns((string?)null);
        var tokenService = new TokenService(_configurationMock.Object);
        var customerId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            tokenService.GenerateToken(customerId, out _));
    }

    [Fact]
    public void GenerateToken_WithCustomExpirationHours_ShouldSetCorrectExpiresAt()
    {
        // Arrange
        _jwtSettingsSectionMock.Setup(x => x["ExpirationHours"]).Returns("12");
        var tokenService = new TokenService(_configurationMock.Object);
        var customerId = Guid.NewGuid();
        var beforeGeneration = DateTime.UtcNow;

        // Act
        tokenService.GenerateToken(customerId, out var expiresAt);

        // Assert
        var afterGeneration = DateTime.UtcNow;
        var expectedMinExpiration = beforeGeneration.AddHours(12);
        var expectedMaxExpiration = afterGeneration.AddHours(12).AddSeconds(5);
        Assert.True(expiresAt >= expectedMinExpiration && expiresAt <= expectedMaxExpiration);
    }

    [Fact]
    public void GenerateToken_WithDefaultExpirationHours_ShouldUse24Hours()
    {
        // Arrange
        _jwtSettingsSectionMock.Setup(x => x["ExpirationHours"]).Returns((string?)null);
        var tokenService = new TokenService(_configurationMock.Object);
        var customerId = Guid.NewGuid();
        var beforeGeneration = DateTime.UtcNow;

        // Act
        tokenService.GenerateToken(customerId, out var expiresAt);

        // Assert
        var afterGeneration = DateTime.UtcNow;
        var expectedMinExpiration = beforeGeneration.AddHours(24);
        var expectedMaxExpiration = afterGeneration.AddHours(24).AddSeconds(5);
        Assert.True(expiresAt >= expectedMinExpiration && expiresAt <= expectedMaxExpiration);
    }

    [Fact]
    public void GenerateToken_WithZeroExpirationHours_ShouldThrowArgumentException()
    {
        // Arrange
        _jwtSettingsSectionMock.Setup(x => x["ExpirationHours"]).Returns("0");
        var tokenService = new TokenService(_configurationMock.Object);
        var customerId = Guid.NewGuid();

        // Act & Assert
        // JWT não permite expiração no passado ou igual a now
        Assert.Throws<ArgumentException>(() =>
            tokenService.GenerateToken(customerId, out _));
    }

    [Fact]
    public void GenerateToken_WithNegativeExpirationHours_ShouldThrowArgumentException()
    {
        // Arrange
        _jwtSettingsSectionMock.Setup(x => x["ExpirationHours"]).Returns("-5");
        var tokenService = new TokenService(_configurationMock.Object);
        var customerId = Guid.NewGuid();

        // Act & Assert
        // JWT não permite expiração no passado
        Assert.Throws<ArgumentException>(() =>
            tokenService.GenerateToken(customerId, out _));
    }

    [Fact]
    public void GenerateToken_ShouldGenerateUniqueTokensForSameCustomer()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        // Act
        var token1 = _tokenService.GenerateToken(customerId, out _);
        var token2 = _tokenService.GenerateToken(customerId, out _);

        // Assert
        Assert.NotEqual(token1, token2);
    }

    [Fact]
    public void GenerateToken_ShouldGenerateDifferentJtiForEachToken()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        // Act
        var token1 = _tokenService.GenerateToken(customerId, out _);
        var token2 = _tokenService.GenerateToken(customerId, out _);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jsonToken1 = handler.ReadJwtToken(token1);
        var jsonToken2 = handler.ReadJwtToken(token2);
        var jti1 = jsonToken1.Claims.First(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
        var jti2 = jsonToken2.Claims.First(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

        Assert.NotEqual(jti1, jti2);
    }

    [Fact]
    public void GenerateToken_ShouldHaveExpirationInFuture()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        // Act
        _tokenService.GenerateToken(customerId, out var expiresAt);

        // Assert
        Assert.True(expiresAt > DateTime.UtcNow);
    }

    [Fact]
    public void GenerateToken_ShouldHaveIatInPastOrPresent()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var beforeGeneration = DateTime.UtcNow;

        // Act
        var token = _tokenService.GenerateToken(customerId, out _);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);
        var iatClaim = jsonToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Iat);
        var iatValue = long.Parse(iatClaim.Value);
        var iatDateTime = DateTimeOffset.FromUnixTimeSeconds(iatValue).DateTime;

        Assert.True(iatDateTime <= DateTime.UtcNow.AddSeconds(5));
        Assert.True(iatDateTime >= beforeGeneration.AddSeconds(-1));
    }

    [Fact]
    public void GenerateToken_ShouldIncludeCorrectCustomerIdClaim()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        // Act
        var token = _tokenService.GenerateToken(customerId, out _);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);
        var customerIdClaim = jsonToken.Claims.First(c => c.Type == "customerId");

        Assert.Equal(customerId.ToString(), customerIdClaim.Value);
    }

    [Fact]
    public void GenerateToken_ShouldIncludeCorrectSubClaim()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        // Act
        var token = _tokenService.GenerateToken(customerId, out _);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);
        var subClaim = jsonToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub);

        Assert.Equal(customerId.ToString(), subClaim.Value);
    }

    [Fact]
    public void GenerateToken_ShouldHaveValidJtiFormat()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        // Act
        var token = _tokenService.GenerateToken(customerId, out _);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);
        var jtiClaim = jsonToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Jti);

        Assert.True(Guid.TryParse(jtiClaim.Value, out _));
    }

    [Fact]
    public void GenerateToken_ShouldHaveValidIatFormat()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        // Act
        var token = _tokenService.GenerateToken(customerId, out _);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);
        var iatClaim = jsonToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Iat);

        Assert.True(long.TryParse(iatClaim.Value, out _));
        Assert.Equal(ClaimValueTypes.Integer64, iatClaim.ValueType);
    }
}

