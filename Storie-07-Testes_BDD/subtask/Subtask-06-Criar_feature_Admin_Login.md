# Subtask 06: Criar feature e steps para Admin Login

## Descrição
Criar feature file e step definitions para o cenário crítico de autenticação de admin via endpoint `/admin/login` com AWS Cognito.

## Passos de implementação
- Criar arquivo `tests/FastFood.Auth.Tests.Bdd/Features/AdminAuthentication.feature`
- Adicionar cenário:
```gherkin
Feature: Admin Authentication
  Como um administrador
  Eu quero autenticar-me via AWS Cognito
  Para que eu possa obter tokens de acesso administrativo

  Scenario: Autenticar admin com credenciais válidas
    Given que eu sou um administrador com credenciais válidas
    When eu faço uma requisição POST para "/api/admin/login" com username e password
    Then a resposta deve ter status 200
    And a resposta deve conter um AccessToken
    And a resposta deve conter um IdToken
    And a resposta deve conter ExpiresIn
    And a resposta deve conter TokenType "Bearer"

  Scenario: Tentar autenticar admin com credenciais inválidas
    Given que eu tenho credenciais inválidas
    When eu faço uma requisição POST para "/api/admin/login" com credenciais inválidas
    Then a resposta deve ter status 401
    And a resposta deve indicar que as credenciais são inválidas
```
- Criar arquivo `tests/FastFood.Auth.Tests.Bdd/StepDefinitions/AdminAuthenticationSteps.cs`
- Implementar steps:
  - `[Given(@"que eu sou um administrador com credenciais válidas")]` - Setup credenciais válidas (mock Cognito)
  - `[Given(@"que eu tenho credenciais inválidas")]` - Setup credenciais inválidas
  - `[When(@"eu faço uma requisição POST para ""(.*)"" com username e password")]` - Fazer requisição
  - `[And(@"a resposta deve conter um AccessToken")]` - Validar AccessToken
  - `[And(@"a resposta deve conter um IdToken")]` - Validar IdToken
  - `[And(@"a resposta deve conter ExpiresIn")]` - Validar ExpiresIn
  - `[And(@"a resposta deve conter TokenType ""(.*)""")]` - Validar TokenType
- Mockar ICognitoService para testes (não usar Cognito real em testes)

## Como testar
- Executar `dotnet test --filter "FullyQualifiedName~AdminAuthentication"` (deve passar)
- Verificar que ambos os cenários executam
- Validar que credenciais válidas retornam tokens e inválidas retornam 401

## Critérios de aceite
- Arquivo AdminAuthentication.feature criado com cenários
- Step definitions implementadas para autenticação admin
- Teste valida autenticação com credenciais válidas
- Teste valida erro 401 para credenciais inválidas
- Teste valida estrutura da resposta (AccessToken, IdToken, ExpiresIn, TokenType)
- CognitoService mockado (não usa Cognito real)
- Todos os cenários passam com sucesso

