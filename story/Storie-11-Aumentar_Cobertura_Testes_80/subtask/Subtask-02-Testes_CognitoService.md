# Subtask 02: Criar testes unitários para CognitoService

## Descrição
Criar suite completa de testes unitários para `CognitoService`, cobrindo todos os cenários de autenticação, tratamento de erros e configuração.

## Arquivo a Testar
- `src/FastFood.Auth.Infra/Services/CognitoService.cs`

## Passos de Implementação

1. **Criar arquivo de testes:**
   - `src/tests/FastFood.Auth.Tests.Unit/Infra/Services/CognitoServiceTests.cs`

2. **Configurar mocks necessários:**
   - Mock de `IConfiguration`
   - Mock de `AmazonCognitoIdentityProviderClient` (usar interface ou wrapper se necessário)
   - Mock de `AdminInitiateAuthResponse`

3. **Cenários de teste a implementar:**

   **a) Construtor e Configuração:**
   - `Constructor_WithValidConfiguration_ShouldCreateClient`
   - `Constructor_WithMissingRegion_ShouldThrowInvalidOperationException`
   - `Constructor_WithExplicitCredentials_ShouldUseBasicCredentials`
   - `Constructor_WithSessionToken_ShouldUseSessionCredentials`
   - `Constructor_WithoutExplicitCredentials_ShouldUseDefaultChain`

   **b) Autenticação Bem-Sucedida:**
   - `AuthenticateAsync_WithValidCredentials_ShouldReturnTokens`
   - `AuthenticateAsync_ShouldReturnCorrectTokenType`
   - `AuthenticateAsync_ShouldReturnCorrectExpiresIn`
   - `AuthenticateAsync_ShouldReturnAccessTokenAndIdToken`

   **c) Tratamento de Exceções do Cognito:**
   - `AuthenticateAsync_WithInvalidCredentials_ShouldThrowUnauthorizedAccessException`
   - `AuthenticateAsync_WithNotAuthorizedException_ShouldThrowUnauthorizedAccessException`
   - `AuthenticateAsync_WithUserNotFoundException_ShouldThrowUnauthorizedAccessException`
   - `AuthenticateAsync_WithInvalidPasswordException_ShouldThrowUnauthorizedAccessException`
   - `AuthenticateAsync_WithExpiredAWSCredentials_ShouldThrowInvalidOperationException`
   - `AuthenticateAsync_WithGenericCognitoException_ShouldThrowInvalidOperationException`

   **d) Configuração Inválida:**
   - `AuthenticateAsync_WithMissingUserPoolId_ShouldThrowInvalidOperationException`
   - `AuthenticateAsync_WithMissingClientId_ShouldThrowInvalidOperationException`
   - `AuthenticateAsync_WithNullAuthenticationResult_ShouldThrowUnauthorizedAccessException`

   **e) Variáveis de Ambiente:**
   - `AuthenticateAsync_WithEnvironmentVariables_ShouldUseEnvironmentVariables`
   - `Constructor_WithEnvironmentVariableRegion_ShouldUseEnvironmentVariable`

## Estrutura do Teste

```csharp
public class CognitoServiceTests
{
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<IConfigurationSection> _cognitoSectionMock;
    private readonly CognitoService _cognitoService;

    public CognitoServiceTests()
    {
        _configurationMock = new Mock<IConfiguration>();
        _cognitoSectionMock = new Mock<IConfigurationSection>();
        // Setup mocks...
    }

    [Fact]
    public async Task AuthenticateAsync_WithValidCredentials_ShouldReturnTokens()
    {
        // Arrange
        var username = "admin@test.com";
        var password = "Password123!";
        
        // Setup mocks para retornar resposta válida
        
        // Act
        var result = await _cognitoService.AuthenticateAsync(username, password);
        
        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.AccessToken);
        Assert.NotEmpty(result.IdToken);
        Assert.Equal("Bearer", result.TokenType);
        Assert.True(result.ExpiresIn > 0);
    }
    
    // Mais testes...
}
```

## Desafios e Soluções

**Desafio 1:** `AmazonCognitoIdentityProviderClient` não tem interface
- **Solução:** Criar wrapper interface `ICognitoIdentityProviderClient` ou usar `HttpClient` mockado
- **Alternativa:** Usar biblioteca como `Moq.Contrib.HttpClient` ou criar abstração

**Desafio 2:** Testar construtor com diferentes configurações
- **Solução:** Criar factory method ou usar `TestHelper` para setup de configuração

**Desafio 3:** Mockar `AdminInitiateAuthResponse`
- **Solução:** Criar objeto de resposta mockado com todas as propriedades necessárias

## Arquivos Afetados

- `src/tests/FastFood.Auth.Tests.Unit/Infra/Services/CognitoServiceTests.cs` (novo)
- Pode ser necessário criar interface wrapper para `AmazonCognitoIdentityProviderClient`

## Como Testar

- Executar `dotnet test --filter "FullyQualifiedName~CognitoServiceTests"`
- Verificar cobertura com `dotnet test /p:CollectCoverage=true`
- Validar que cobertura de `CognitoService` >= 80%

## Critérios de Aceite

- [ ] Arquivo de testes criado em `Infra/Services/`
- [ ] Pelo menos 15 casos de teste implementados
- [ ] Todos os métodos públicos testados
- [ ] Todos os cenários de erro testados
- [ ] Cobertura de `CognitoService` >= 80%
- [ ] Todos os testes passam
- [ ] Mocks configurados corretamente (sem dependências externas)

## Notas Técnicas

- Usar `Moq` para mockar `IConfiguration`
- Para `AmazonCognitoIdentityProviderClient`, considerar criar wrapper ou usar biblioteca de teste AWS
- Testes devem ser isolados e não fazer chamadas reais ao AWS
- Focar em testar a lógica de negócio, não a integração com AWS SDK


