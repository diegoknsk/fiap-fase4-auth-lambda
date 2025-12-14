# Subtask 04: Criar testes unitários para RegisterCustomerUseCase

## Descrição
Criar testes unitários para RegisterCustomerUseCase cobrindo cenários: customer existe e customer não existe.

## Passos de implementação
- Criar arquivo `tests/FastFood.Auth.Tests.Unit/UseCases/Customer/RegisterCustomerUseCaseTests.cs`
- Criar método `ExecuteAsync_WhenCustomerExists_ShouldReturnExistingCustomerToken`
- Criar método `ExecuteAsync_WhenCustomerNotExists_ShouldCreateNewCustomerAndReturnToken`
- Usar Moq para mockar ICustomerRepository e ITokenService
- Verificar comportamentos esperados em cada cenário

## Como testar
- Executar `dotnet test` (deve passar)
- Verificar cobertura de código

## Critérios de aceite
- Testes criados para ambos os cenários
- Testes usam mocks corretamente
- `dotnet test` passa com sucesso

