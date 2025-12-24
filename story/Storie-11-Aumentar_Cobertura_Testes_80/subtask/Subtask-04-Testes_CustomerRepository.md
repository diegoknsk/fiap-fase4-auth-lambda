# Subtask 04: Criar testes unitários para CustomerRepository

## Descrição
Criar suite completa de testes unitários para `CustomerRepository`, cobrindo todos os métodos de acesso a dados e mapeamento entre Domain e Entity.

## Arquivo a Testar
- `src/FastFood.Auth.Infra.Persistence/Repositories/CustomerRepository.cs`

## Passos de Implementação

1. **Criar arquivo de testes:**
   - `src/tests/FastFood.Auth.Tests.Unit/Infra.Persistence/Repositories/CustomerRepositoryTests.cs`

2. **Configurar InMemory Database:**
   - Usar `Microsoft.EntityFrameworkCore.InMemory` para testes
   - Criar `AuthDbContext` com opções InMemory

3. **Cenários de teste a implementar:**

   **a) GetByIdAsync:**
   - `GetByIdAsync_WithExistingId_ShouldReturnCustomer`
   - `GetByIdAsync_WithNonExistingId_ShouldReturnNull`
   - `GetByIdAsync_ShouldMapEntityToDomainCorrectly`
   - `GetByIdAsync_WithCustomerWithEmail_ShouldMapEmailCorrectly`
   - `GetByIdAsync_WithCustomerWithCpf_ShouldMapCpfCorrectly`
   - `GetByIdAsync_WithCustomerWithoutEmailAndCpf_ShouldMapNullsCorrectly`
   - `GetByIdAsync_ShouldPreserveCreatedAt`

   **b) GetByCpfAsync:**
   - `GetByCpfAsync_WithExistingCpf_ShouldReturnCustomer`
   - `GetByCpfAsync_WithNonExistingCpf_ShouldReturnNull`
   - `GetByCpfAsync_ShouldMapEntityToDomainCorrectly`
   - `GetByCpfAsync_WithFormattedCpf_ShouldFindCustomer`

   **c) ExistsByCpfAsync:**
   - `ExistsByCpfAsync_WithExistingCpf_ShouldReturnTrue`
   - `ExistsByCpfAsync_WithNonExistingCpf_ShouldReturnFalse`

   **d) AddAsync:**
   - `AddAsync_WithNewCustomer_ShouldSaveAndReturnCustomer`
   - `AddAsync_WithCustomerWithEmail_ShouldSaveEmail`
   - `AddAsync_WithCustomerWithCpf_ShouldSaveCpf`
   - `AddAsync_WithAnonymousCustomer_ShouldSaveWithoutEmailAndCpf`
   - `AddAsync_ShouldMapDomainToEntityCorrectly`
   - `AddAsync_ShouldSetCreatedAt`
   - `AddAsync_ShouldReturnCustomerWithId`

   **e) Mapeamento Domain <-> Entity:**
   - `MapToDomain_WithFullCustomer_ShouldMapAllProperties`
   - `MapToDomain_WithCustomerWithoutEmail_ShouldMapNullEmail`
   - `MapToDomain_WithCustomerWithoutCpf_ShouldMapNullCpf`
   - `MapToDomain_ShouldPreserveCreatedAt`
   - `MapToEntity_WithFullCustomer_ShouldMapAllProperties`
   - `MapToEntity_WithCustomerWithoutEmail_ShouldMapNullEmail`
   - `MapToEntity_WithCustomerWithoutCpf_ShouldMapNullCpf`
   - `MapToEntity_ShouldPreserveCreatedAt`

   **f) CustomerType:**
   - `AddAsync_WithRegisteredCustomer_ShouldSaveCustomerType`
   - `AddAsync_WithAnonymousCustomer_ShouldSaveCustomerType`
   - `GetByIdAsync_ShouldMapCustomerTypeCorrectly`

## Estrutura do Teste

```csharp
public class CustomerRepositoryTests : IDisposable
{
    private readonly AuthDbContext _context;
    private readonly CustomerRepository _repository;

    public CustomerRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AuthDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        _context = new AuthDbContext(options);
        _repository = new CustomerRepository(_context);
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingId_ShouldReturnCustomer()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var entity = new CustomerEntity
        {
            Id = customerId,
            Name = "Test Customer",
            Email = "test@example.com",
            Cpf = "12345678901",
            CustomerType = (int)CustomerTypeEnum.Registered,
            CreatedAt = DateTime.UtcNow
        };
        
        _context.Customers.Add(entity);
        await _context.SaveChangesAsync();
        
        // Act
        var result = await _repository.GetByIdAsync(customerId);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(customerId, result.Id);
        Assert.Equal("Test Customer", result.Name);
        Assert.NotNull(result.Email);
        Assert.Equal("test@example.com", result.Email.Value);
        Assert.NotNull(result.Cpf);
        Assert.Equal("12345678901", result.Cpf.Value);
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistingId_ShouldReturnNull()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();
        
        // Act
        var result = await _repository.GetByIdAsync(nonExistingId);
        
        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_WithNewCustomer_ShouldSaveAndReturnCustomer()
    {
        // Arrange
        var customer = new Customer(
            id: Guid.NewGuid(),
            name: "New Customer",
            email: new Email("new@example.com"),
            cpf: new Cpf("12345678901"),
            customerType: CustomerTypeEnum.Registered
        );
        
        // Act
        var result = await _repository.AddAsync(customer);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(customer.Id, result.Id);
        Assert.Equal("New Customer", result.Name);
        
        // Verificar que foi salvo no banco
        var savedEntity = await _context.Customers.FindAsync(customer.Id);
        Assert.NotNull(savedEntity);
        Assert.Equal("New Customer", savedEntity.Name);
    }

    [Fact]
    public async Task GetByCpfAsync_WithExistingCpf_ShouldReturnCustomer()
    {
        // Arrange
        var cpf = "12345678901";
        var entity = new CustomerEntity
        {
            Id = Guid.NewGuid(),
            Name = "Test Customer",
            Cpf = cpf,
            CustomerType = (int)CustomerTypeEnum.Registered,
            CreatedAt = DateTime.UtcNow
        };
        
        _context.Customers.Add(entity);
        await _context.SaveChangesAsync();
        
        // Act
        var result = await _repository.GetByCpfAsync(cpf);
        
        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Cpf);
        Assert.Equal(cpf, result.Cpf.Value);
    }

    [Fact]
    public async Task ExistsByCpfAsync_WithExistingCpf_ShouldReturnTrue()
    {
        // Arrange
        var cpf = "12345678901";
        var entity = new CustomerEntity
        {
            Id = Guid.NewGuid(),
            Cpf = cpf,
            CustomerType = (int)CustomerTypeEnum.Registered,
            CreatedAt = DateTime.UtcNow
        };
        
        _context.Customers.Add(entity);
        await _context.SaveChangesAsync();
        
        // Act
        var result = await _repository.ExistsByCpfAsync(cpf);
        
        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExistsByCpfAsync_WithNonExistingCpf_ShouldReturnFalse()
    {
        // Arrange
        var nonExistingCpf = "99999999999";
        
        // Act
        var result = await _repository.ExistsByCpfAsync(nonExistingCpf);
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task AddAsync_WithAnonymousCustomer_ShouldSaveWithoutEmailAndCpf()
    {
        // Arrange
        var customer = new Customer(
            id: Guid.NewGuid(),
            name: null,
            email: null,
            cpf: null,
            customerType: CustomerTypeEnum.Anonymous
        );
        
        // Act
        var result = await _repository.AddAsync(customer);
        
        // Assert
        Assert.NotNull(result);
        Assert.Null(result.Email);
        Assert.Null(result.Cpf);
        Assert.Equal(CustomerTypeEnum.Anonymous, result.CustomerType);
        
        // Verificar no banco
        var savedEntity = await _context.Customers.FindAsync(customer.Id);
        Assert.NotNull(savedEntity);
        Assert.Null(savedEntity.Email);
        Assert.Null(savedEntity.Cpf);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
```

## Dependências Necessárias

- `Microsoft.EntityFrameworkCore.InMemory` (adicionar ao projeto de testes)
- `FastFood.Auth.Infra.Persistence` (referência ao projeto)

## Arquivos Afetados

- `src/tests/FastFood.Auth.Tests.Unit/Infra.Persistence/Repositories/CustomerRepositoryTests.cs` (novo)
- `src/tests/FastFood.Auth.Tests.Unit/FastFood.Auth.Tests.Unit.csproj` (adicionar referência e pacote)

## Como Testar

- Executar `dotnet test --filter "FullyQualifiedName~CustomerRepositoryTests"`
- Verificar cobertura com `dotnet test /p:CollectCoverage=true`
- Validar que cobertura de `CustomerRepository` >= 80%

## Critérios de Aceite

- [ ] Arquivo de testes criado em `Infra.Persistence/Repositories/`
- [ ] Pelo menos 25 casos de teste implementados
- [ ] Todos os métodos públicos testados
- [ ] InMemory Database configurado corretamente
- [ ] Testes de mapeamento Domain <-> Entity implementados
- [ ] Testes de cenários com e sem Email/CPF implementados
- [ ] Cobertura de `CustomerRepository` >= 80%
- [ ] Todos os testes passam
- [ ] Testes são isolados (cada teste usa database separado)

## Notas Técnicas

- Usar `UseInMemoryDatabase` com nome único para cada teste (Guid)
- Implementar `IDisposable` para limpar contexto após cada teste
- Testar mapeamento bidirecional (Domain -> Entity e Entity -> Domain)
- Validar preservação de `CreatedAt` usando reflection
- Testar todos os tipos de Customer (Registered, Anonymous)



