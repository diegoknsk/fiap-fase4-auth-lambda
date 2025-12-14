# Subtask 04: Criar testes unitários para UseCases de Customer

## Descrição
Criar testes unitários para os UseCases de Customer (CreateAnonymousCustomer, RegisterCustomer, IdentifyCustomer) usando Moq para mockar dependências.

## Passos de implementação
- Criar diretório `tests/FastFood.Auth.Tests.Unit/UseCases/Customer/` se não existir
- Criar arquivo `CreateAnonymousCustomerUseCaseTests.cs` com testes:
  - `ExecuteAsync_ShouldCreateAnonymousCustomer_AndReturnToken` - Deve criar customer anônimo e retornar token
  - `ExecuteAsync_ShouldCallRepositoryAddAsync` - Deve chamar repository para adicionar
  - `ExecuteAsync_ShouldCallTokenServiceGenerateToken` - Deve chamar token service
  - `ExecuteAsync_ShouldReturnResponseWithTokenAndCustomerId` - Deve retornar resposta com token
- Criar arquivo `RegisterCustomerUseCaseTests.cs` com testes:
  - `ExecuteAsync_WhenCustomerExists_ShouldReturnExistingCustomerToken` - Se customer existe, retorna token existente
  - `ExecuteAsync_WhenCustomerNotExists_ShouldCreateNewCustomer` - Se não existe, cria novo
  - `ExecuteAsync_WhenCustomerNotExists_ShouldCallRepositoryAddAsync` - Deve chamar repository
  - `ExecuteAsync_ShouldValidateCpf` - Deve validar CPF usando Value Object
  - `ExecuteAsync_WhenCustomerExists_ShouldNotCreateDuplicate` - Não deve criar duplicado
- Criar arquivo `IdentifyCustomerUseCaseTests.cs` com testes:
  - `ExecuteAsync_WhenCustomerExists_ShouldReturnToken` - Se customer existe, retorna token
  - `ExecuteAsync_WhenCustomerNotExists_ShouldThrowUnauthorizedAccessException` - Se não existe, lança exceção
  - `ExecuteAsync_ShouldValidateCpf` - Deve validar CPF
  - `ExecuteAsync_ShouldCallRepositoryGetByCpfAsync` - Deve chamar repository
  - `ExecuteAsync_ShouldCallTokenServiceWhenCustomerFound` - Deve chamar token service quando encontrado

## Como testar
- Executar `dotnet test --filter "FullyQualifiedName~UseCases.Customer"` (deve passar todos os testes)
- Verificar que mocks são configurados corretamente
- Validar que todos os cenários estão cobertos

## Critérios de aceite
- Arquivo CreateAnonymousCustomerUseCaseTests.cs criado com pelo menos 4 testes
- Arquivo RegisterCustomerUseCaseTests.cs criado com pelo menos 5 testes
- Arquivo IdentifyCustomerUseCaseTests.cs criado com pelo menos 5 testes
- Todos os testes usam Moq para mockar ICustomerRepository e ITokenService
- Testes validam chamadas aos mocks (Verify)
- Testes cobrem cenários de sucesso e erro
- Todos os testes passando
- `dotnet test` passa com sucesso

