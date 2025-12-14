# Subtask 03: Criar feature e steps para Customer Anonymous

## Descrição
Criar feature file e step definitions para o cenário crítico de criação de customer anônimo via endpoint `/customer/anonymous`.

## Passos de implementação
- Criar arquivo `tests/FastFood.Auth.Tests.Bdd/Features/CustomerAuthentication.feature`
- Adicionar cenário:
```gherkin
Feature: Customer Authentication
  Como um cliente
  Eu quero criar um customer anônimo
  Para que eu possa continuar com o processo sem me identificar

  Scenario: Criar customer anônimo e receber token
    Given que eu sou um cliente
    When eu faço uma requisição POST para "/api/customer/anonymous"
    Then a resposta deve ter status 200
    And a resposta deve conter um token JWT válido
    And a resposta deve conter um customerId (Guid)
    And um customer deve ser criado no banco com CustomerType = Anonymous
```
- Criar arquivo `tests/FastFood.Auth.Tests.Bdd/StepDefinitions/CustomerAuthenticationSteps.cs`
- Implementar steps:
  - `[Given(@"que eu sou um cliente")]` - Setup inicial
  - `[When(@"eu faço uma requisição POST para ""(.*)""")]` - Fazer requisição HTTP
  - `[Then(@"a resposta deve ter status (\d+)")]` - Validar status code
  - `[And(@"a resposta deve conter um token JWT válido")]` - Validar token
  - `[And(@"a resposta deve conter um customerId \(Guid\)")]` - Validar customerId
  - `[And(@"um customer deve ser criado no banco com CustomerType = Anonymous")]` - Validar banco
- Usar HttpClient ou WebApplicationFactory para fazer requisições HTTP
- Validar resposta JSON e estrutura do token JWT

## Como testar
- Executar `dotnet test --filter "FullyQualifiedName~CustomerAuthentication"` (deve passar)
- Verificar que o cenário executa completamente
- Validar que todas as assertions passam

## Critérios de aceite
- Arquivo CustomerAuthentication.feature criado com cenário de customer anônimo
- Step definitions implementadas para o cenário
- Teste faz requisição HTTP real para o endpoint
- Teste valida status code 200
- Teste valida token JWT na resposta
- Teste valida customerId na resposta
- Teste valida criação no banco (se usando banco de teste)
- Cenário passa com sucesso

