# Subtask 06: Adicionar cenários de erro no UseCase de Admin

## Descrição
Adicionar testes adicionais para cenários de erro e casos de borda no UseCase de Admin (AuthenticateAdminUseCase).

## Arquivo a Testar
- `src/FastFood.Auth.Application/UseCases/Admin/AuthenticateAdminUseCase.cs`

## Passos de Implementação

1. **Analisar testes existentes:**
   - Verificar quais cenários já estão cobertos
   - Identificar lacunas de cobertura

2. **Adicionar testes para AuthenticateAdminUseCase:**

   **a) Cenários Existentes (verificar se já existem):**
   - `ExecuteAsync_WithValidCredentials_ShouldReturnTokens`
   - `ExecuteAsync_ShouldReturnAccessToken`
   - `ExecuteAsync_ShouldReturnIdToken`
   - `ExecuteAsync_ShouldReturnExpiresIn`
   - `ExecuteAsync_ShouldReturnTokenType`

   **b) Cenários de Erro:**
   - `ExecuteAsync_WithInvalidCredentials_ShouldPropagateUnauthorizedAccessException`
   - `ExecuteAsync_WhenCognitoServiceFails_ShouldPropagateException`
   - `ExecuteAsync_WhenCognitoServiceThrowsUnauthorizedAccessException_ShouldPropagate`
   - `ExecuteAsync_WhenCognitoServiceThrowsInvalidOperationException_ShouldPropagate`
   - `ExecuteAsync_WithNullInputModel_ShouldThrowArgumentNullException`
   - `ExecuteAsync_WithNullUsername_ShouldHandleGracefully`
   - `ExecuteAsync_WithNullPassword_ShouldHandleGracefully`
   - `ExecuteAsync_WithEmptyUsername_ShouldHandleGracefully`
   - `ExecuteAsync_WithEmptyPassword_ShouldHandleGracefully`

   **c) Cenários de Validação:**
   - `ExecuteAsync_ShouldCallCognitoServiceWithCorrectParameters`
   - `ExecuteAsync_ShouldCallPresenterWithCorrectOutputModel`
   - `ExecuteAsync_ShouldMapCognitoResultToOutputModel`

   **d) Cenários de Borda:**
   - `ExecuteAsync_WithVeryLongUsername_ShouldHandleGracefully`
   - `ExecuteAsync_WithVeryLongPassword_ShouldHandleGracefully`
   - `ExecuteAsync_WithSpecialCharactersInUsername_ShouldHandleGracefully`
   - `ExecuteAsync_WithSpecialCharactersInPassword_ShouldHandleGracefully`

## Estrutura dos Testes Adicionais

```csharp
[Fact]
public async Task ExecuteAsync_WhenCognitoServiceFails_ShouldPropagateException()
{
    // Arrange
    var cognitoServiceMock = new Mock<ICognitoService>();
    var presenterMock = new Mock<AuthenticateAdminPresenter>();
    
    cognitoServiceMock
        .Setup(c => c.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
        .ThrowsAsync(new InvalidOperationException("Cognito service error"));
    
    var useCase = new AuthenticateAdminUseCase(
        cognitoServiceMock.Object,
        presenterMock.Object
    );
    
    var inputModel = new AuthenticateAdminInputModel
    {
        Username = "admin@test.com",
        Password = "Password123!"
    };
    
    // Act & Assert
    var exception = await Assert.ThrowsAsync<InvalidOperationException>(
        () => useCase.ExecuteAsync(inputModel)
    );
    
    Assert.Equal("Cognito service error", exception.Message);
    presenterMock.Verify(p => p.Present(It.IsAny<AuthenticateAdminOutputModel>()), Times.Never);
}

[Fact]
public async Task ExecuteAsync_WithInvalidCredentials_ShouldPropagateUnauthorizedAccessException()
{
    // Arrange
    var cognitoServiceMock = new Mock<ICognitoService>();
    var presenterMock = new Mock<AuthenticateAdminPresenter>();
    
    cognitoServiceMock
        .Setup(c => c.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
        .ThrowsAsync(new UnauthorizedAccessException("Credenciais inválidas"));
    
    var useCase = new AuthenticateAdminUseCase(
        cognitoServiceMock.Object,
        presenterMock.Object
    );
    
    var inputModel = new AuthenticateAdminInputModel
    {
        Username = "admin@test.com",
        Password = "WrongPassword"
    };
    
    // Act & Assert
    var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
        () => useCase.ExecuteAsync(inputModel)
    );
    
    Assert.Equal("Credenciais inválidas", exception.Message);
    presenterMock.Verify(p => p.Present(It.IsAny<AuthenticateAdminOutputModel>()), Times.Never);
}

[Fact]
public async Task ExecuteAsync_ShouldCallCognitoServiceWithCorrectParameters()
{
    // Arrange
    var cognitoServiceMock = new Mock<ICognitoService>();
    var presenterMock = new Mock<AuthenticateAdminPresenter>();
    
    var expectedUsername = "admin@test.com";
    var expectedPassword = "Password123!";
    
    var cognitoResult = new AuthenticateAdminResult
    {
        AccessToken = "access-token",
        IdToken = "id-token",
        ExpiresIn = 3600,
        TokenType = "Bearer"
    };
    
    cognitoServiceMock
        .Setup(c => c.AuthenticateAsync(expectedUsername, expectedPassword))
        .ReturnsAsync(cognitoResult);
    
    presenterMock
        .Setup(p => p.Present(It.IsAny<AuthenticateAdminOutputModel>()))
        .Returns((AuthenticateAdminOutputModel model) => model);
    
    var useCase = new AuthenticateAdminUseCase(
        cognitoServiceMock.Object,
        presenterMock.Object
    );
    
    var inputModel = new AuthenticateAdminInputModel
    {
        Username = expectedUsername,
        Password = expectedPassword
    };
    
    // Act
    await useCase.ExecuteAsync(inputModel);
    
    // Assert
    cognitoServiceMock.Verify(
        c => c.AuthenticateAsync(expectedUsername, expectedPassword),
        Times.Once
    );
}

[Fact]
public async Task ExecuteAsync_ShouldMapCognitoResultToOutputModel()
{
    // Arrange
    var cognitoServiceMock = new Mock<ICognitoService>();
    var presenterMock = new Mock<AuthenticateAdminPresenter>();
    
    var cognitoResult = new AuthenticateAdminResult
    {
        AccessToken = "access-token-123",
        IdToken = "id-token-456",
        ExpiresIn = 7200,
        TokenType = "Bearer"
    };
    
    cognitoServiceMock
        .Setup(c => c.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
        .ReturnsAsync(cognitoResult);
    
    AuthenticateAdminOutputModel? capturedOutputModel = null;
    presenterMock
        .Setup(p => p.Present(It.IsAny<AuthenticateAdminOutputModel>()))
        .Callback<AuthenticateAdminOutputModel>(model => capturedOutputModel = model)
        .Returns((AuthenticateAdminOutputModel model) => model);
    
    var useCase = new AuthenticateAdminUseCase(
        cognitoServiceMock.Object,
        presenterMock.Object
    );
    
    var inputModel = new AuthenticateAdminInputModel
    {
        Username = "admin@test.com",
        Password = "Password123!"
    };
    
    // Act
    await useCase.ExecuteAsync(inputModel);
    
    // Assert
    Assert.NotNull(capturedOutputModel);
    Assert.Equal(cognitoResult.AccessToken, capturedOutputModel.AccessToken);
    Assert.Equal(cognitoResult.IdToken, capturedOutputModel.IdToken);
    Assert.Equal(cognitoResult.ExpiresIn, capturedOutputModel.ExpiresIn);
    Assert.Equal(cognitoResult.TokenType, capturedOutputModel.TokenType);
}

[Fact]
public async Task ExecuteAsync_WithNullInputModel_ShouldThrowArgumentNullException()
{
    // Arrange
    var cognitoServiceMock = new Mock<ICognitoService>();
    var presenterMock = new Mock<AuthenticateAdminPresenter>();
    
    var useCase = new AuthenticateAdminUseCase(
        cognitoServiceMock.Object,
        presenterMock.Object
    );
    
    // Act & Assert
    await Assert.ThrowsAsync<ArgumentNullException>(
        () => useCase.ExecuteAsync(null!)
    );
    
    cognitoServiceMock.Verify(
        c => c.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()),
        Times.Never
    );
}
```

## Arquivos Afetados

- `src/tests/FastFood.Auth.Tests.Unit/UseCases/Admin/AuthenticateAdminUseCaseTests.cs` (atualizar)

## Como Testar

- Executar `dotnet test --filter "FullyQualifiedName~AuthenticateAdminUseCase"`
- Verificar cobertura com `dotnet test /p:CollectCoverage=true`
- Validar que cobertura do UseCase de Admin >= 80%

## Critérios de Aceite

- [ ] Pelo menos 10 novos casos de teste adicionados
- [ ] Todos os cenários de erro testados
- [ ] Validações de entrada testadas (null, empty)
- [ ] Propagação de exceções testada
- [ ] Mapeamento de resultado testado
- [ ] Verificação de chamadas aos mocks implementada
- [ ] Cobertura do UseCase de Admin >= 80%
- [ ] Todos os testes passam
- [ ] Mocks configurados corretamente para simular falhas

## Notas Técnicas

- Usar `Moq` para simular falhas no CognitoService
- Testar propagação de diferentes tipos de exceções
- Validar que quando CognitoService falha, o UseCase propaga a exceção
- Usar `Callback` para capturar parâmetros passados ao Presenter
- Verificar que métodos não são chamados quando validação falha antes
- Testar mapeamento completo de `AuthenticateAdminResult` para `AuthenticateAdminOutputModel`



