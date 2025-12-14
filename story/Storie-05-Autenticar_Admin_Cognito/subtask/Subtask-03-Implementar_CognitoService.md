# Subtask 03: Implementar CognitoService na Infra

## Descrição
Implementar classe CognitoService que implementa ICognitoService usando AWSSDK.CognitoIdentityProvider para autenticar via AdminInitiateAuthRequest.

## Passos de implementação
- Criar arquivo `src/FastFood.Auth.Infra/Services/CognitoService.cs` (ou em Infra.Persistence)
- Criar classe `CognitoService` implementando `ICognitoService`
- No construtor, criar `AmazonCognitoIdentityProviderClient` usando região de variável de ambiente
- Implementar método `AuthenticateAsync`:
  - Ler variáveis de ambiente: COGNITO__REGION, COGNITO__USERPOOLID, COGNITO__CLIENTID
  - Criar `AdminInitiateAuthRequest` com AuthFlowType.ADMIN_USER_PASSWORD_AUTH
  - Chamar `AdminInitiateAuthAsync`
  - Retornar tokens (AccessToken, IdToken, ExpiresIn)
  - Tratar exceções (NotAuthorizedException retorna erro, outras retornam erro genérico)

## Como testar
- Executar `dotnet build` (deve compilar sem erros)
- Verificar que a classe implementa corretamente a interface

## Critérios de aceite
- Arquivo CognitoService.cs criado
- Classe implementa ICognitoService
- Método AuthenticateAsync implementado
- Usa AdminInitiateAuthRequest corretamente
- Trata exceções adequadamente
- Código compila sem erros

