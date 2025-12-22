# Subtask 07: Adicionar testes de validação e erro nos Controllers

## Descrição
Adicionar testes adicionais para validação de entrada, tratamento de erros e diferentes status codes nos Controllers (CustomerController e AdminController).

## Arquivos a Testar
- `src/FastFood.Auth.Lambda/Controllers/CustomerController.cs`
- `src/FastFood.Auth.Lambda/Controllers/AdminController.cs`

## Passos de Implementação

1. **Analisar testes existentes:**
   - Verificar quais cenários já estão cobertos
   - Identificar lacunas de cobertura

2. **Adicionar testes para CustomerController:**

   **a) Cenários de Validação:**
   - `CreateAnonymous_ShouldReturn200Ok`
   - `Register_WithValidInput_ShouldReturn200Ok`
   - `Register_WithInvalidModelState_ShouldReturn400BadRequest`
   - `Register_WithNullInput_ShouldReturn400BadRequest`
   - `Identify_WithValidInput_ShouldReturn200Ok`
   - `Identify_WithInvalidModelState_ShouldReturn400BadRequest`
   - `Identify_WithNullInput_ShouldReturn400BadRequest`

   **b) Cenários de Erro:**
   - `CreateAnonymous_WhenUseCaseThrowsException_ShouldReturn500InternalServerError`
   - `Register_WhenUseCaseThrowsDomainException_ShouldReturn500InternalServerError`
   - `Register_WhenUseCaseThrowsException_ShouldReturn500InternalServerError`
   - `Identify_WhenUseCaseThrowsUnauthorizedAccessException_ShouldReturn401Unauthorized`
   - `Identify_WhenUseCaseThrowsDomainException_ShouldReturn500InternalServerError`
   - `Identify_WhenUseCaseThrowsException_ShouldReturn500InternalServerError`

   **c) Cenários de Response:**
   - `CreateAnonymous_ShouldReturnCorrectResponseModel`
   - `Register_ShouldReturnCorrectResponseModel`
   - `Identify_ShouldReturnCorrectResponseModel`
   - `CreateAnonymous_ShouldCallUseCaseOnce`
   - `Register_ShouldCallUseCaseWithCorrectInput`
   - `Identify_ShouldCallUseCaseWithCorrectInput`

3. **Adicionar testes para AdminController:**

   **a) Cenários de Validação:**
   - `Login_WithValidInput_ShouldReturn200Ok`
   - `Login_WithInvalidModelState_ShouldReturn400BadRequest`
   - `Login_WithNullInput_ShouldReturn400BadRequest`

   **b) Cenários de Erro:**
   - `Login_WhenUseCaseThrowsUnauthorizedAccessException_ShouldReturn401Unauthorized`
   - `Login_WhenUseCaseThrowsException_ShouldReturn500InternalServerError`
   - `Login_WhenUseCaseThrowsInvalidOperationException_ShouldReturn500InternalServerError`

   **c) Cenários de Response:**
   - `Login_ShouldReturnCorrectResponseModel`
   - `Login_ShouldCallUseCaseWithCorrectInput`
   - `Login_ShouldReturnCorrectStatusCodes`

## Estrutura dos Testes Adicionais

### CustomerController

```csharp
[Fact]
public async Task Register_WithInvalidModelState_ShouldReturn400BadRequest()
{
    // Arrange
    var createAnonymousUseCaseMock = new Mock<CreateAnonymousCustomerUseCase>();
    var registerUseCaseMock = new Mock<RegisterCustomerUseCase>();
    var identifyUseCaseMock = new Mock<IdentifyCustomerUseCase>();
    
    var controller = new CustomerController(
        createAnonymousUseCaseMock.Object,
        registerUseCaseMock.Object,
        identifyUseCaseMock.Object
    );
    
    controller.ModelState.AddModelError("Cpf", "CPF is required");
    var inputModel = new RegisterCustomerInputModel();
    
    // Act
    var result = await controller.Register(inputModel);
    
    // Assert
    var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
    Assert.Equal(400, badRequestResult.StatusCode);
    registerUseCaseMock.Verify(u => u.ExecuteAsync(It.IsAny<RegisterCustomerInputModel>()), Times.Never);
}

[Fact]
public async Task Identify_WhenUseCaseThrowsUnauthorizedAccessException_ShouldReturn401Unauthorized()
{
    // Arrange
    var createAnonymousUseCaseMock = new Mock<CreateAnonymousCustomerUseCase>();
    var registerUseCaseMock = new Mock<RegisterCustomerUseCase>();
    var identifyUseCaseMock = new Mock<IdentifyCustomerUseCase>();
    
    identifyUseCaseMock
        .Setup(u => u.ExecuteAsync(It.IsAny<IdentifyCustomerInputModel>()))
        .ThrowsAsync(new UnauthorizedAccessException("Customer not found"));
    
    var controller = new CustomerController(
        createAnonymousUseCaseMock.Object,
        registerUseCaseMock.Object,
        identifyUseCaseMock.Object
    );
    
    var inputModel = new IdentifyCustomerInputModel
    {
        Cpf = "12345678901"
    };
    
    // Act
    var result = await controller.Identify(inputModel);
    
    // Assert
    var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
    Assert.Equal(401, unauthorizedResult.StatusCode);
    
    var response = unauthorizedResult.Value as dynamic;
    Assert.NotNull(response);
}

[Fact]
public async Task CreateAnonymous_WhenUseCaseThrowsException_ShouldReturn500InternalServerError()
{
    // Arrange
    var createAnonymousUseCaseMock = new Mock<CreateAnonymousCustomerUseCase>();
    var registerUseCaseMock = new Mock<RegisterCustomerUseCase>();
    var identifyUseCaseMock = new Mock<IdentifyCustomerUseCase>();
    
    createAnonymousUseCaseMock
        .Setup(u => u.ExecuteAsync())
        .ThrowsAsync(new InvalidOperationException("Database error"));
    
    var controller = new CustomerController(
        createAnonymousUseCaseMock.Object,
        registerUseCaseMock.Object,
        identifyUseCaseMock.Object
    );
    
    // Act
    var result = await controller.CreateAnonymous();
    
    // Assert
    var statusCodeResult = Assert.IsType<ObjectResult>(result);
    Assert.Equal(500, statusCodeResult.StatusCode);
    
    var response = statusCodeResult.Value as dynamic;
    Assert.NotNull(response);
}

[Fact]
public async Task Register_ShouldCallUseCaseWithCorrectInput()
{
    // Arrange
    var createAnonymousUseCaseMock = new Mock<CreateAnonymousCustomerUseCase>();
    var registerUseCaseMock = new Mock<RegisterCustomerUseCase>();
    var identifyUseCaseMock = new Mock<IdentifyCustomerUseCase>();
    
    var expectedInput = new RegisterCustomerInputModel
    {
        Cpf = "12345678901"
    };
    
    var outputModel = new RegisterCustomerOutputModel
    {
        Token = "token",
        CustomerId = Guid.NewGuid(),
        ExpiresAt = DateTime.UtcNow.AddHours(24)
    };
    
    registerUseCaseMock
        .Setup(u => u.ExecuteAsync(expectedInput))
        .ReturnsAsync(outputModel);
    
    var controller = new CustomerController(
        createAnonymousUseCaseMock.Object,
        registerUseCaseMock.Object,
        identifyUseCaseMock.Object
    );
    
    // Act
    var result = await controller.Register(expectedInput);
    
    // Assert
    registerUseCaseMock.Verify(
        u => u.ExecuteAsync(expectedInput),
        Times.Once
    );
    
    var okResult = Assert.IsType<OkObjectResult>(result);
    Assert.Equal(outputModel, okResult.Value);
}
```

### AdminController

```csharp
[Fact]
public async Task Login_WhenUseCaseThrowsUnauthorizedAccessException_ShouldReturn401Unauthorized()
{
    // Arrange
    var authenticateUseCaseMock = new Mock<AuthenticateAdminUseCase>();
    
    authenticateUseCaseMock
        .Setup(u => u.ExecuteAsync(It.IsAny<AuthenticateAdminInputModel>()))
        .ThrowsAsync(new UnauthorizedAccessException("Credenciais inválidas"));
    
    var controller = new AdminController(authenticateUseCaseMock.Object);
    
    var inputModel = new AuthenticateAdminInputModel
    {
        Username = "admin@test.com",
        Password = "WrongPassword"
    };
    
    // Act
    var result = await controller.Login(inputModel);
    
    // Assert
    var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
    Assert.Equal(401, unauthorizedResult.StatusCode);
    
    var response = unauthorizedResult.Value as dynamic;
    Assert.NotNull(response);
}

[Fact]
public async Task Login_WithInvalidModelState_ShouldReturn400BadRequest()
{
    // Arrange
    var authenticateUseCaseMock = new Mock<AuthenticateAdminUseCase>();
    
    var controller = new AdminController(authenticateUseCaseMock.Object);
    controller.ModelState.AddModelError("Username", "Username is required");
    
    var inputModel = new AuthenticateAdminInputModel();
    
    // Act
    var result = await controller.Login(inputModel);
    
    // Assert
    var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
    Assert.Equal(400, badRequestResult.StatusCode);
    authenticateUseCaseMock.Verify(u => u.ExecuteAsync(It.IsAny<AuthenticateAdminInputModel>()), Times.Never);
}

[Fact]
public async Task Login_ShouldReturnCorrectResponseModel()
{
    // Arrange
    var authenticateUseCaseMock = new Mock<AuthenticateAdminUseCase>();
    
    var expectedOutput = new AuthenticateAdminOutputModel
    {
        AccessToken = "access-token",
        IdToken = "id-token",
        ExpiresIn = 3600,
        TokenType = "Bearer"
    };
    
    authenticateUseCaseMock
        .Setup(u => u.ExecuteAsync(It.IsAny<AuthenticateAdminInputModel>()))
        .ReturnsAsync(expectedOutput);
    
    var controller = new AdminController(authenticateUseCaseMock.Object);
    
    var inputModel = new AuthenticateAdminInputModel
    {
        Username = "admin@test.com",
        Password = "Password123!"
    };
    
    // Act
    var result = await controller.Login(inputModel);
    
    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    var response = Assert.IsType<AuthenticateAdminOutputModel>(okResult.Value);
    Assert.Equal(expectedOutput.AccessToken, response.AccessToken);
    Assert.Equal(expectedOutput.IdToken, response.IdToken);
    Assert.Equal(expectedOutput.ExpiresIn, response.ExpiresIn);
    Assert.Equal(expectedOutput.TokenType, response.TokenType);
}
```

## Arquivos Afetados

- `src/tests/FastFood.Auth.Tests.Unit/Controllers/CustomerControllerTests.cs` (atualizar)
- `src/tests/FastFood.Auth.Tests.Unit/Controllers/AdminControllerTests.cs` (atualizar)

## Como Testar

- Executar `dotnet test --filter "FullyQualifiedName~ControllerTests"`
- Verificar cobertura com `dotnet test /p:CollectCoverage=true`
- Validar que cobertura dos Controllers >= 70%

## Critérios de Aceite

- [ ] Pelo menos 20 novos casos de teste adicionados
- [ ] Todos os status codes testados (200, 400, 401, 500)
- [ ] Validação de ModelState testada
- [ ] Tratamento de diferentes tipos de exceções testado
- [ ] Verificação de chamadas aos UseCases implementada
- [ ] Validação de response models implementada
- [ ] Cobertura dos Controllers >= 70%
- [ ] Todos os testes passam
- [ ] Mocks configurados corretamente

## Notas Técnicas

- Usar `Moq` para mockar UseCases
- Testar `ModelState` adicionando erros manualmente
- Validar status codes usando `Assert.IsType<T>()`
- Testar mensagens de erro retornadas
- Verificar que UseCases são chamados com parâmetros corretos
- Testar cenários de null e empty em inputs


