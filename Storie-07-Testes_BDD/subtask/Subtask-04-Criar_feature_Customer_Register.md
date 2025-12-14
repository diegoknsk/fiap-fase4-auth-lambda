# Subtask 04: Criar feature e steps para Customer Register

## Descrição
Criar feature file e step definitions para o cenário crítico de registro de customer via endpoint `/customer/register`.

## Passos de implementação
- Adicionar cenário ao arquivo `CustomerAuthentication.feature`:
```gherkin
  Scenario: Registrar customer com CPF válido
    Given que eu tenho um CPF válido "12345678901"
    When eu faço uma requisição POST para "/api/customer/register" com o CPF
    Then a resposta deve ter status 200
    And a resposta deve conter um token JWT válido
    And a resposta deve conter um customerId (Guid)
    And um customer deve ser criado no banco com CustomerType = Registered
    And o customer deve ter o CPF informado

  Scenario: Registrar customer com CPF já existente
    Given que já existe um customer com CPF "12345678901"
    When eu faço uma requisição POST para "/api/customer/register" com o mesmo CPF
    Then a resposta deve ter status 200
    And a resposta deve conter um token JWT válido
    And a resposta deve conter o customerId do customer existente
    And nenhum customer duplicado deve ser criado
```
- Adicionar step definitions em `CustomerAuthenticationSteps.cs`:
  - `[Given(@"que eu tenho um CPF válido ""(.*)""")]` - Setup CPF válido
  - `[Given(@"que já existe um customer com CPF ""(.*)""")]` - Criar customer existente
  - `[When(@"eu faço uma requisição POST para ""(.*)"" com o CPF")]` - Fazer requisição com body
  - `[And(@"o customer deve ter o CPF informado")]` - Validar CPF no banco
  - `[And(@"nenhum customer duplicado deve ser criado")]` - Validar não duplicação

## Como testar
- Executar `dotnet test --filter "FullyQualifiedName~CustomerAuthentication"` (deve passar)
- Verificar que ambos os cenários executam
- Validar que customer novo é criado e customer existente retorna token

## Critérios de aceite
- Cenários de registro adicionados ao arquivo .feature
- Step definitions implementadas para registro
- Teste valida criação de customer novo
- Teste valida retorno de token para customer existente
- Teste valida que não há duplicação
- Todos os cenários passam com sucesso

