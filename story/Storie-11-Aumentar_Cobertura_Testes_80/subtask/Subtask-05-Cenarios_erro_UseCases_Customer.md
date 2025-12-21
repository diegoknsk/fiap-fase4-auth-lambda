# Subtask 05: Adicionar cenários de erro nos UseCases de Customer

## Descrição
Adicionar testes adicionais para cenários de erro e casos de borda nos UseCases de Customer (CreateAnonymousCustomerUseCase, RegisterCustomerUseCase, IdentifyCustomerUseCase).

## Arquivos a Testar
- `src/FastFood.Auth.Application/UseCases/Customer/CreateAnonymousCustomerUseCase.cs`
- `src/FastFood.Auth.Application/UseCases/Customer/RegisterCustomerUseCase.cs`
- `src/FastFood.Auth.Application/UseCases/Customer/IdentifyCustomerUseCase.cs`

## Passos de Implementação

1. **Analisar testes existentes:**
   - Verificar quais cenários já estão cobertos
   - Identificar lacunas de cobertura

2. **Adicionar testes para CreateAnonymousCustomerUseCase:**

   **a) Cenários de Sucesso (se não existirem):**
   - `ExecuteAsync_ShouldCreateAnonymousCustomer`
   - `ExecuteAsync_ShouldReturnToken`
   - `ExecuteAsync_ShouldReturnCustomerId`
   - `ExecuteAsync_ShouldReturnExpiresAt`

   **b) Cenários de Erro:**
   - `ExecuteAsync_WhenRepositoryFails_ShouldPropagateException`
   - `ExecuteAsync_WhenTokenServiceFails_ShouldPropagateException`
   - `ExecuteAsync_WhenRepositoryThrowsException_ShouldNotCreateCustomer`

3. **Adicionar testes para RegisterCustomerUseCase:**

   **a) Cenários Existentes (verificar se já existem):**
   - `ExecuteAsync_WithNewCustomer_ShouldCreateAndReturnToken`
   - `ExecuteAsync_WithExistingCustomer_ShouldReturnExistingCustomerToken`

   **b) Cenários de Erro e Validação:**
   - `ExecuteAsync_WithInvalidCpf_ShouldThrowDomainException`
   - `ExecuteAsync_WithNullCpf_ShouldThrowDomainException`
   - `ExecuteAsync_WithEmptyCpf_ShouldThrowDomainException`
   - `ExecuteAsync_WhenRepositoryFails_ShouldPropagateException`
   - `ExecuteAsync_WhenTokenServiceFails_ShouldPropagateException`
   - `ExecuteAsync_WithExistingCustomer_ShouldNotCreateDuplicate`

   **c) Cenários de Borda:**
   - `ExecuteAsync_WithCpfWithSpecialCharacters_ShouldNormalizeCpf`
   - `ExecuteAsync_WithCpfWithSpaces_ShouldNormalizeCpf`
   - `ExecuteAsync_WithFormattedCpf_ShouldNormalizeCpf`

4. **Adicionar testes para IdentifyCustomerUseCase:**

   **a) Cenários Existentes (verificar se já existem):**
   - `ExecuteAsync_WithExistingCustomer_ShouldReturnToken`
   - `ExecuteAsync_WithNonExistingCustomer_ShouldThrowUnauthorizedAccessException`

   **b) Cenários de Erro e Validação:**
   - `ExecuteAsync_WithInvalidCpf_ShouldThrowDomainException`
   - `ExecuteAsync_WithNullCpf_ShouldThrowDomainException`
   - `ExecuteAsync_WithEmptyCpf_ShouldThrowDomainException`
   - `ExecuteAsync_WhenRepositoryFails_ShouldPropagateException`
   - `ExecuteAsync_WhenTokenServiceFails_ShouldPropagateException`
   - `ExecuteAsync_WithNonExistingCustomer_ShouldThrowUnauthorizedAccessExceptionWithCorrectMessage`

   **c) Cenários de Borda:**
   - `ExecuteAsync_WithCpfWithSpecialCharacters_ShouldNormalizeCpf`
   - `ExecuteAsync_WithCpfWithSpaces_ShouldNormalizeCpf`
   - `ExecuteAsync_WithFormattedCpf_ShouldNormalizeCpf`

## Estrutura dos Testes Adicionais

### CreateAnonymousCustomerUseCase

```csharp
[Fact]
public async Task ExecuteAsync_WhenRepositoryFails_ShouldPropagateException()
{
    // Arrange
    var repositoryMock = new Mock<ICustomerRepository>();
    var tokenServiceMock = new Mock<ITokenService>();
    var presenterMock = new Mock<CreateAnonymousCustomerPresenter>();
    
    repositoryMock
        .Setup(r => r.AddAsync(It.IsAny<Customer>()))
        .ThrowsAsync(new InvalidOperationException("Database error"));
    
    var useCase = new CreateAnonymousCustomerUseCase(
        repositoryMock.Object,
        tokenServiceMock.Object,
        presenterMock.Object
    );
    
    // Act & Assert
    await Assert.ThrowsAsync<InvalidOperationException>(
        () => useCase.ExecuteAsync()
    );
    
    tokenServiceMock.Verify(t => t.GenerateToken(It.IsAny<Guid>(), out It.Ref<DateTime>.IsAny), Times.Never);
}
```

### RegisterCustomerUseCase

```csharp
[Fact]
public async Task ExecuteAsync_WithInvalidCpf_ShouldThrowDomainException()
{
    // Arrange
    var repositoryMock = new Mock<ICustomerRepository>();
    var tokenServiceMock = new Mock<ITokenService>();
    var presenterMock = new Mock<RegisterCustomerPresenter>();
    
    var useCase = new RegisterCustomerUseCase(
        repositoryMock.Object,
        tokenServiceMock.Object,
        presenterMock.Object
    );
    
    var inputModel = new RegisterCustomerInputModel
    {
        Cpf = "00000000000" // CPF inválido
    };
    
    // Act & Assert
    await Assert.ThrowsAsync<DomainException>(
        () => useCase.ExecuteAsync(inputModel)
    );
    
    repositoryMock.Verify(r => r.GetByCpfAsync(It.IsAny<string>()), Times.Never);
}

[Fact]
public async Task ExecuteAsync_WhenRepositoryFails_ShouldPropagateException()
{
    // Arrange
    var repositoryMock = new Mock<ICustomerRepository>();
    var tokenServiceMock = new Mock<ITokenService>();
    var presenterMock = new Mock<RegisterCustomerPresenter>();
    
    repositoryMock
        .Setup(r => r.GetByCpfAsync(It.IsAny<string>()))
        .ThrowsAsync(new InvalidOperationException("Database error"));
    
    var useCase = new RegisterCustomerUseCase(
        repositoryMock.Object,
        tokenServiceMock.Object,
        presenterMock.Object
    );
    
    var inputModel = new RegisterCustomerInputModel
    {
        Cpf = "12345678901" // CPF válido
    };
    
    // Act & Assert
    await Assert.ThrowsAsync<InvalidOperationException>(
        () => useCase.ExecuteAsync(inputModel)
    );
}
```

### IdentifyCustomerUseCase

```csharp
[Fact]
public async Task ExecuteAsync_WithNonExistingCustomer_ShouldThrowUnauthorizedAccessExceptionWithCorrectMessage()
{
    // Arrange
    var repositoryMock = new Mock<ICustomerRepository>();
    var tokenServiceMock = new Mock<ITokenService>();
    var presenterMock = new Mock<IdentifyCustomerPresenter>();
    
    repositoryMock
        .Setup(r => r.GetByCpfAsync(It.IsAny<string>()))
        .ReturnsAsync((Customer?)null);
    
    var useCase = new IdentifyCustomerUseCase(
        repositoryMock.Object,
        tokenServiceMock.Object,
        presenterMock.Object
    );
    
    var inputModel = new IdentifyCustomerInputModel
    {
        Cpf = "12345678901"
    };
    
    // Act
    var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
        () => useCase.ExecuteAsync(inputModel)
    );
    
    // Assert
    Assert.Equal("Customer not found with the provided CPF.", exception.Message);
    tokenServiceMock.Verify(t => t.GenerateToken(It.IsAny<Guid>(), out It.Ref<DateTime>.IsAny), Times.Never);
}

[Fact]
public async Task ExecuteAsync_WithInvalidCpf_ShouldThrowDomainException()
{
    // Arrange
    var repositoryMock = new Mock<ICustomerRepository>();
    var tokenServiceMock = new Mock<ITokenService>();
    var presenterMock = new Mock<IdentifyCustomerPresenter>();
    
    var useCase = new IdentifyCustomerUseCase(
        repositoryMock.Object,
        tokenServiceMock.Object,
        presenterMock.Object
    );
    
    var inputModel = new IdentifyCustomerInputModel
    {
        Cpf = "00000000000" // CPF inválido
    };
    
    // Act & Assert
    await Assert.ThrowsAsync<DomainException>(
        () => useCase.ExecuteAsync(inputModel)
    );
    
    repositoryMock.Verify(r => r.GetByCpfAsync(It.IsAny<string>()), Times.Never);
}
```

## Arquivos Afetados

- `src/tests/FastFood.Auth.Tests.Unit/UseCases/Customer/CreateAnonymousCustomerUseCaseTests.cs` (atualizar)
- `src/tests/FastFood.Auth.Tests.Unit/UseCases/Customer/RegisterCustomerUseCaseTests.cs` (atualizar)
- `src/tests/FastFood.Auth.Tests.Unit/UseCases/Customer/IdentifyCustomerUseCaseTests.cs` (atualizar)

## Como Testar

- Executar `dotnet test --filter "FullyQualifiedName~CustomerUseCase"`
- Verificar cobertura com `dotnet test /p:CollectCoverage=true`
- Validar que cobertura dos UseCases de Customer >= 80%

## Critérios de Aceite

- [ ] Pelo menos 15 novos casos de teste adicionados
- [ ] Todos os cenários de erro testados
- [ ] Validações de entrada testadas
- [ ] Propagação de exceções testada
- [ ] Cenários de borda testados (CPF formatado, com espaços, etc.)
- [ ] Cobertura dos UseCases de Customer >= 80%
- [ ] Todos os testes passam
- [ ] Mocks configurados corretamente para simular falhas

## Notas Técnicas

- Usar `Moq` para simular falhas nos repositórios e serviços
- Testar propagação de exceções (não devem ser engolidas)
- Validar que quando uma dependência falha, o UseCase propaga a exceção
- Testar normalização de CPF (remoção de caracteres especiais)
- Verificar que métodos não são chamados quando validação falha antes

