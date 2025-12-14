# Subtask 04: Criar UseCase AuthenticateAdminUseCase

## Descrição
Criar UseCase AuthenticateAdminUseCase que orquestra a autenticação admin usando ICognitoService.

## Passos de implementação
- Criar diretório `src/FastFood.Auth.Application/UseCases/Admin/` se não existir
- Criar arquivo `src/FastFood.Auth.Application/UseCases/Admin/AuthenticateAdminUseCase.cs`
- Criar classe `AuthenticateAdminUseCase` recebendo `ICognitoService` via construtor
- Criar método `ExecuteAsync(AuthenticateAdminCommand command)` que:
  - Chama `_cognitoService.AuthenticateAsync(command.Username, command.Password)`
  - Retorna `AuthenticateAdminResponse` com tokens
- Criar `AuthenticateAdminCommand` com Username e Password
- Criar `AuthenticateAdminResponse` com AccessToken, IdToken, ExpiresIn, TokenType

## Como testar
- Executar `dotnet build` no projeto Application (deve compilar sem erros)

## Critérios de aceite
- Arquivo AuthenticateAdminUseCase.cs criado
- UseCase chama CognitoService
- Retorna resposta com tokens
- Código compila sem erros

