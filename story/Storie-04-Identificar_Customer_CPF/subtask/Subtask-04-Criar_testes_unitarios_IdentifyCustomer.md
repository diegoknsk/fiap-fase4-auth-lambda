# Subtask 04: Criar testes unitários para IdentifyCustomerUseCase

## Descrição
Criar testes unitários para IdentifyCustomerUseCase cobrindo cenários: customer encontrado e customer não encontrado.

## Passos de implementação
- Criar arquivo `tests/FastFood.Auth.Tests.Unit/UseCases/Customer/IdentifyCustomerUseCaseTests.cs`
- Criar método `ExecuteAsync_WhenCustomerExists_ShouldReturnToken`
- Criar método `ExecuteAsync_WhenCustomerNotExists_ShouldThrowUnauthorizedAccessException`
- Usar Moq para mockar ICustomerRepository e ITokenService
- Verificar comportamentos esperados em cada cenário

## Como testar
- Executar `dotnet test` (deve passar)
- Verificar cobertura de código

## Critérios de aceite
- Testes criados para ambos os cenários
- Teste verifica exceção quando customer não existe
- `dotnet test` passa com sucesso

