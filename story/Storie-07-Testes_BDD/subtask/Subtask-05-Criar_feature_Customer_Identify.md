# Subtask 05: Criar feature e steps para Customer Identify

## Descrição
Criar feature file e step definitions para o cenário crítico de identificação de customer via endpoint `/customer/identify`.

## Passos de implementação
- Adicionar cenário ao arquivo `CustomerAuthentication.feature`:
```gherkin
  Scenario: Identificar customer existente por CPF
    Given que existe um customer registrado com CPF "12345678901"
    When eu faço uma requisição POST para "/api/customer/identify" com o CPF
    Then a resposta deve ter status 200
    And a resposta deve conter um token JWT válido
    And a resposta deve conter o customerId do customer existente

  Scenario: Tentar identificar customer inexistente
    Given que não existe nenhum customer com CPF "99999999999"
    When eu faço uma requisição POST para "/api/customer/identify" com o CPF
    Then a resposta deve ter status 401
    And a resposta deve indicar que o customer não foi encontrado
```
- Adicionar step definitions em `CustomerAuthenticationSteps.cs`:
  - `[Given(@"que existe um customer registrado com CPF ""(.*)""")]` - Criar customer no banco
  - `[Given(@"que não existe nenhum customer com CPF ""(.*)""")]` - Garantir que não existe
  - `[And(@"a resposta deve conter o customerId do customer existente")]` - Validar customerId correto
  - `[And(@"a resposta deve indicar que o customer não foi encontrado")]` - Validar mensagem de erro

## Como testar
- Executar `dotnet test --filter "FullyQualifiedName~CustomerAuthentication"` (deve passar)
- Verificar que ambos os cenários executam
- Validar que customer existente retorna token e inexistente retorna 401

## Critérios de aceite
- Cenários de identificação adicionados ao arquivo .feature
- Step definitions implementadas para identificação
- Teste valida identificação de customer existente
- Teste valida erro 401 para customer inexistente
- Todos os cenários passam com sucesso

