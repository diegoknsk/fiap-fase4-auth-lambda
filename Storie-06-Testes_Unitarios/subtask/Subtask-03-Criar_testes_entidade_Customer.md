# Subtask 03: Criar testes unitários para entidade Customer

## Descrição
Criar testes unitários para a entidade Customer do Domain, validando criação, propriedades e regras de domínio.

## Passos de implementação
- Criar diretório `tests/FastFood.Auth.Tests.Unit/Domain/Entities/` se não existir
- Criar arquivo `CustomerTests.cs` com testes:
  - `Constructor_WithValidData_ShouldCreateCustomer` - Criar customer com dados válidos
  - `Constructor_WithRegisteredType_ShouldSetCustomerTypeRegistered` - CustomerType = Registered
  - `Constructor_WithAnonymousType_ShouldSetCustomerTypeAnonymous` - CustomerType = Anonymous
  - `Constructor_ShouldSetCreatedAtToUtcNow` - CreatedAt deve ser preenchido automaticamente
  - `AddCustomer_ShouldGenerateNewGuid` - AddCustomer deve gerar novo Guid
  - `AddCustomer_ShouldSetCreatedAt` - AddCustomer deve setar CreatedAt
  - `AddCustomer_WithRegisteredType_ShouldCreateRegisteredCustomer` - Criar customer Registered
  - `AddCustomer_WithAnonymousType_ShouldCreateAnonymousCustomer` - Criar customer Anonymous
  - `Customer_WithNullCpf_ShouldBeValid` - Customer pode ter Cpf null (anônimo)
  - `Customer_WithNullEmail_ShouldBeValid` - Customer pode ter Email null
  - `Customer_WithNullName_ShouldBeValid` - Customer pode ter Name null

## Como testar
- Executar `dotnet test --filter "FullyQualifiedName~CustomerTests"` (deve passar todos os testes)
- Verificar que todos os cenários de criação estão cobertos
- Validar que propriedades são setadas corretamente

## Critérios de aceite
- Arquivo CustomerTests.cs criado com pelo menos 10 testes
- Testes cobrem criação via construtor e AddCustomer
- Testes validam CustomerType (Registered e Anonymous)
- Testes validam CreatedAt automático
- Testes validam que propriedades nullable podem ser null
- Todos os testes passando
- `dotnet test` passa com sucesso

