# Subtask 03: Criar testes unitários para TokenService

## Descrição
Criar suite completa de testes unitários para `TokenService`, cobrindo geração de tokens JWT, validação de configuração e diferentes cenários de expiração.

## Arquivo a Testar
- `src/FastFood.Auth.Infra/Services/TokenService.cs`

## Passos de Implementação

1. **Criar arquivo de testes:**
   - `src/tests/FastFood.Auth.Tests.Unit/Infra/Services/TokenServiceTests.cs`

2. **Configurar mocks necessários:**
   - Mock de `IConfiguration`
   - Mock de `IConfigurationSection` para JwtSettings

3. **Cenários de teste a implementar:**

   **a) Geração de Token Válido:**
   - `GenerateToken_WithValidConfiguration_ShouldReturnValidToken`
   - `GenerateToken_ShouldSetCorrectExpiresAt`
   - `GenerateToken_ShouldIncludeCustomerIdInClaims`
   - `GenerateToken_ShouldIncludeJtiClaim`
   - `GenerateToken_ShouldIncludeIatClaim`
   - `GenerateToken_ShouldIncludeSubClaim`
   - `GenerateToken_ShouldHaveCorrectIssuer`
   - `GenerateToken_ShouldHaveCorrectAudience`
   - `GenerateToken_ShouldUseHmacSha256Algorithm`

   **b) Configuração Inválida:**
   - `GenerateToken_WithMissingSecret_ShouldThrowInvalidOperationException`
   - `GenerateToken_WithMissingIssuer_ShouldThrowInvalidOperationException`
   - `GenerateToken_WithMissingAudience_ShouldThrowInvalidOperationException`

   **c) Diferentes Valores de Expiração:**
   - `GenerateToken_WithCustomExpirationHours_ShouldSetCorrectExpiresAt`
   - `GenerateToken_WithDefaultExpirationHours_ShouldUse24Hours`
   - `GenerateToken_WithZeroExpirationHours_ShouldUse24Hours`
   - `GenerateToken_WithNegativeExpirationHours_ShouldUse24Hours`

   **d) Validação de Token:**
   - `GenerateToken_ShouldGenerateUniqueTokensForSameCustomer`
   - `GenerateToken_ShouldGenerateDifferentJtiForEachToken`
   - `GenerateToken_ShouldHaveExpirationInFuture`
   - `GenerateToken_ShouldHaveIatInPastOrPresent`

   **e) Claims do Token:**
   - `GenerateToken_ShouldIncludeCorrectCustomerIdClaim`
   - `GenerateToken_ShouldIncludeCorrectSubClaim`
   - `GenerateToken_ShouldHaveValidJtiFormat`
   - `GenerateToken_ShouldHaveValidIatFormat`

## Estrutura do Teste

```csharp
public class TokenServiceTests
{
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<IConfigurationSection> _jwtSettingsSectionMock;
    private readonly TokenService _tokenService;

    public TokenServiceTests()
    {
        _configurationMock = new Mock<IConfiguration>();
        _jwtSettingsSectionMock = new Mock<IConfigurationSection>();
        
        // Setup default configuration
        _jwtSettingsSectionMock.Setup(x => x["Secret"])
            .Returns("MySuperSecretKeyThatIsAtLeast32CharactersLong");
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
    public void GenerateToken_WithMissingSecret_ShouldThrowInvalidOperationException()
    {
        // Arrange
        _jwtSettingsSectionMock.Setup(x => x["Secret"]).Returns((string?)null);
        var tokenService = new TokenService(_configurationMock.Object);
        var customerId = Guid.NewGuid();
        
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => 
            tokenService.GenerateToken(customerId, out _));
    }
    
    // Mais testes...
}
```

## Validação de Token JWT

Para validar o token gerado, usar `JwtSecurityTokenHandler`:

```csharp
var handler = new JwtSecurityTokenHandler();
var jsonToken = handler.ReadJwtToken(token);

// Validar claims
var customerIdClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == "customerId");
var subClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == "sub");
var jtiClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == "jti");
var iatClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == "iat");

// Validar expiração
Assert.True(jsonToken.ValidTo > DateTime.UtcNow);
```

## Arquivos Afetados

- `src/tests/FastFood.Auth.Tests.Unit/Infra/Services/TokenServiceTests.cs` (novo)

## Dependências Necessárias

- `System.IdentityModel.Tokens.Jwt` (já deve estar no projeto)
- `Moq` (já deve estar no projeto)

## Como Testar

- Executar `dotnet test --filter "FullyQualifiedName~TokenServiceTests"`
- Verificar cobertura com `dotnet test /p:CollectCoverage=true`
- Validar que cobertura de `TokenService` >= 80%

## Critérios de Aceite

- [ ] Arquivo de testes criado em `Infra/Services/`
- [ ] Pelo menos 20 casos de teste implementados
- [ ] Todos os métodos públicos testados
- [ ] Todos os cenários de configuração inválida testados
- [ ] Validação de claims do token implementada
- [ ] Validação de expiração implementada
- [ ] Cobertura de `TokenService` >= 80%
- [ ] Todos os testes passam
- [ ] Mocks configurados corretamente

## Notas Técnicas

- Usar `JwtSecurityTokenHandler` para ler e validar tokens
- Secret deve ter pelo menos 32 caracteres para testes
- Validar formato de claims (JTI deve ser GUID, IAT deve ser Unix timestamp)
- Testar diferentes valores de `ExpirationHours` (0, negativo, padrão, customizado)


