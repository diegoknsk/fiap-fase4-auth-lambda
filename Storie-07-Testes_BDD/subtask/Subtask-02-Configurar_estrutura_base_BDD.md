# Subtask 02: Configurar estrutura base para testes BDD

## Descrição
Configurar estrutura base de diretórios e arquivos de configuração para testes BDD, incluindo setup de WebApplicationFactory para testes de API.

## Passos de implementação
- Criar estrutura de diretórios:
  - `tests/FastFood.Auth.Tests.Bdd/Features/` (arquivos .feature)
  - `tests/FastFood.Auth.Tests.Bdd/StepDefinitions/` (step definitions)
  - `tests/FastFood.Auth.Tests.Bdd/Support/` (helpers e fixtures)
- Criar arquivo `tests/FastFood.Auth.Tests.Bdd/Support/WebApplicationFactoryFixture.cs`:
  - Criar classe que herda de `WebApplicationFactory<Program>` (do Lambda)
  - Configurar banco de dados em memória ou TestContainers (opcional)
  - Configurar mocks de serviços externos (Cognito) se necessário
- Criar arquivo `tests/FastFood.Auth.Tests.Bdd/Support/Hooks.cs`:
  - Criar hooks do SpecFlow (BeforeScenario, AfterScenario)
  - Configurar setup e teardown de testes
- Criar arquivo `specflow.json` na raiz do projeto BDD (configuração do SpecFlow)
- Adicionar arquivo `.feature.cs` ao .gitignore (arquivos gerados)

## Como testar
- Executar `dotnet build` no projeto BDD (deve compilar sem erros)
- Verificar que estrutura de diretórios está criada
- Validar que WebApplicationFactory está configurado

## Critérios de aceite
- Estrutura de diretórios criada (Features, StepDefinitions, Support)
- WebApplicationFactoryFixture criado e configurado
- Hooks do SpecFlow criados
- Arquivo specflow.json criado (se necessário)
- `.feature.cs` adicionado ao .gitignore
- Código compila sem erros

