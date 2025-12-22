# Subtask 02: Criar port ICognitoService na Application

## Descrição
Criar interface ICognitoService na camada Application que define o contrato para autenticação via AWS Cognito.

## Passos de implementação
- Criar arquivo `src/FastFood.Auth.Application/Ports/ICognitoService.cs`
- Criar interface `ICognitoService` com método:
  - `Task<AuthenticateAdminResult> AuthenticateAsync(string username, string password)`
- Criar classe `AuthenticateAdminResult` com propriedades: AccessToken, IdToken, ExpiresIn, TokenType

## Como testar
- Executar `dotnet build` no projeto Application (deve compilar sem erros)

## Critérios de aceite
- Arquivo ICognitoService.cs criado
- Interface define método AuthenticateAsync
- Classe AuthenticateAdminResult criada
- Código compila sem erros

