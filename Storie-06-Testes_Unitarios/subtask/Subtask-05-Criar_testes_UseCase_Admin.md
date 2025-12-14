# Subtask 05: Criar testes unitários para UseCase de Admin

## Descrição
Criar testes unitários para o UseCase AuthenticateAdminUseCase usando Moq para mockar ICognitoService.

## Passos de implementação
- Criar diretório `tests/FastFood.Auth.Tests.Unit/UseCases/Admin/` se não existir
- Criar arquivo `AuthenticateAdminUseCaseTests.cs` com testes:
  - `ExecuteAsync_WhenCredentialsValid_ShouldReturnTokens` - Credenciais válidas retornam tokens
  - `ExecuteAsync_ShouldCallCognitoServiceAuthenticateAsync` - Deve chamar CognitoService
  - `ExecuteAsync_WhenCredentialsValid_ShouldReturnAccessTokenAndIdToken` - Retorna AccessToken e IdToken
  - `ExecuteAsync_WhenCredentialsInvalid_ShouldThrowException` - Credenciais inválidas lançam exceção
  - `ExecuteAsync_ShouldPassUsernameAndPasswordToCognitoService` - Passa username e password corretos

## Como testar
- Executar `dotnet test --filter "FullyQualifiedName~UseCases.Admin"` (deve passar todos os testes)
- Verificar que mocks são configurados corretamente
- Validar que exceções são tratadas adequadamente

## Critérios de aceite
- Arquivo AuthenticateAdminUseCaseTests.cs criado com pelo menos 5 testes
- Testes usam Moq para mockar ICognitoService
- Testes validam chamadas aos mocks (Verify)
- Testes cobrem cenários de sucesso e erro
- Testes validam retorno de AccessToken e IdToken
- Todos os testes passando
- `dotnet test` passa com sucesso

