# Subtask 07: Configurar execução de testes BDD no CI/CD

## Descrição
Configurar execução de testes BDD no GitHub Actions, garantindo que os testes sejam executados automaticamente no pipeline de CI/CD.

## Passos de implementação
- Abrir arquivo `.github/workflows/tests.yml` (ou criar se não existir)
- Adicionar job `bdd-tests` ou incluir no job de testes existente:
  - Usar runner: `runs-on: ubuntu-latest`
  - Fazer checkout do código: `actions/checkout@v4`
  - Configurar .NET: `actions/setup-dotnet@v4` com versão `8.0.x`
  - Restaurar dependências: `dotnet restore`
  - Executar testes BDD: `dotnet test tests/FastFood.Auth.Tests.Bdd/FastFood.Auth.Tests.Bdd.csproj`
  - Publicar resultados de testes (opcional): usar action para publicar relatórios
- Configurar variáveis de ambiente necessárias (se houver)
- Adicionar step para gerar relatório de testes BDD (opcional)
- Garantir que testes BDD executam após testes unitários

## Como testar
- Fazer commit e push do workflow
- Verificar que o workflow aparece em Actions no GitHub
- Executar workflow e verificar que testes BDD são executados
- Validar que testes BDD passam no CI/CD
- Verificar logs do workflow para confirmar execução

## Critérios de aceite
- Workflow GitHub Actions configurado para executar testes BDD
- Testes BDD executam automaticamente no CI/CD
- Testes BDD passam no pipeline
- Logs do workflow mostram execução bem-sucedida
- Relatório de testes BDD gerado (se configurado)
- Workflow falha se testes BDD falharem

