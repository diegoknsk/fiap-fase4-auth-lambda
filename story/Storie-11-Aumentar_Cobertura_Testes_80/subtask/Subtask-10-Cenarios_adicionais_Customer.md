# Subtask 10: Adicionar cenários adicionais na entidade Customer

## Descrição
Adicionar testes adicionais para a entidade Customer, cobrindo todos os métodos, construtores e cenários de uso.

## Arquivo a Testar
- `src/FastFood.Auth.Domain/Entities/CustomerIdentification/Customer.cs`

## Passos de Implementação

1. **Analisar testes existentes e identificar lacunas**

2. **Cenários de teste a implementar:**

   **a) Construtor:**
   - `Constructor_WithAllProperties_ShouldSetAllProperties`
   - `Constructor_WithNullName_ShouldSetNullName`
   - `Constructor_WithNullEmail_ShouldSetNullEmail`
   - `Constructor_WithNullCpf_ShouldSetNullCpf`
   - `Constructor_ShouldSetCreatedAt`
   - `Constructor_ShouldSetId`

   **b) AddCustomer:**
   - `AddCustomer_ShouldSetNewId`
   - `AddCustomer_ShouldSetCreatedAt`
   - `AddCustomer_WithAllProperties_ShouldSetAllProperties`
   - `AddCustomer_WithNullProperties_ShouldSetNullProperties`

   **c) Propriedades:**
   - `Id_ShouldBeSet`
   - `Name_ShouldBeSet`
   - `Email_ShouldBeSet`
   - `Cpf_ShouldBeSet`
   - `CustomerType_ShouldBeSet`
   - `CreatedAt_ShouldBeSet`

   **d) Tipos de Customer:**
   - `Constructor_WithRegisteredType_ShouldSetType`
   - `Constructor_WithAnonymousType_ShouldSetType`
   - `AddCustomer_WithRegisteredType_ShouldSetType`
   - `AddCustomer_WithAnonymousType_ShouldSetType`

## Estrutura dos Testes

```csharp
[Fact]
public void Constructor_WithAllProperties_ShouldSetAllProperties()
{
    // Arrange
    var id = Guid.NewGuid();
    var name = "Test Customer";
    var email = new Email("test@example.com");
    var cpf = new Cpf("12345678901");
    var customerType = CustomerTypeEnum.Registered;
    
    // Act
    var customer = new Customer(id, name, email, cpf, customerType);
    
    // Assert
    Assert.Equal(id, customer.Id);
    Assert.Equal(name, customer.Name);
    Assert.Equal(email, customer.Email);
    Assert.Equal(cpf, customer.Cpf);
    Assert.Equal(customerType, customer.CustomerType);
    Assert.True(customer.CreatedAt <= DateTime.UtcNow);
}

[Fact]
public void AddCustomer_ShouldSetNewId()
{
    // Arrange
    var customer = new Customer();
    
    // Act
    customer.AddCustomer("Test", null, null, CustomerTypeEnum.Anonymous);
    
    // Assert
    Assert.NotEqual(Guid.Empty, customer.Id);
}

[Fact]
public void AddCustomer_ShouldSetCreatedAt()
{
    // Arrange
    var customer = new Customer();
    var beforeCreation = DateTime.UtcNow;
    
    // Act
    customer.AddCustomer("Test", null, null, CustomerTypeEnum.Anonymous);
    
    // Assert
    Assert.True(customer.CreatedAt >= beforeCreation);
    Assert.True(customer.CreatedAt <= DateTime.UtcNow);
}
```

## Arquivos Afetados

- `src/tests/FastFood.Auth.Tests.Unit/Domain/Entities/CustomerTests.cs` (atualizar)

## Critérios de Aceite

- [ ] Pelo menos 15 novos casos de teste adicionados
- [ ] Todos os construtores testados
- [ ] Todos os métodos testados
- [ ] Todas as propriedades testadas
- [ ] Cobertura de Customer >= 85%
- [ ] Todos os testes passam

