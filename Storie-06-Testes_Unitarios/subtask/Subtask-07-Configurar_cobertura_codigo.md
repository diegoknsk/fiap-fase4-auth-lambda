# Subtask 07: Configurar cobertura de código e validar >= 80%

## Descrição
Configurar coleta de cobertura de código usando coverlet e validar que a cobertura mínima de 80% é atingida.

## Passos de implementação
- Verificar que pacote `coverlet.collector` já está no .csproj (já existe)
- Executar testes com cobertura: `dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover`
- Verificar cobertura por projeto:
  - Domain: >= 80%
  - Application (UseCases): >= 80%
  - Lambda (Controllers): >= 70% (aceitável para controllers)
- Criar arquivo `.github/workflows/tests.yml` (ou atualizar existente) para executar testes com cobertura no CI/CD
- Adicionar step no workflow para gerar relatório de cobertura
- Configurar threshold mínimo de 80% no coverlet (opcional, via arquivo de configuração)

## Como testar
- Executar `dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover` localmente
- Verificar relatório de cobertura gerado
- Validar que cobertura >= 80% para Domain e Application
- Executar workflow no GitHub Actions e verificar que testes passam

## Critérios de aceite
- Cobertura de código coletada com sucesso
- Cobertura do Domain >= 80%
- Cobertura da Application (UseCases) >= 80%
- Cobertura do Lambda (Controllers) >= 70%
- Workflow GitHub Actions configurado para executar testes com cobertura
- Relatório de cobertura gerado no CI/CD
- `dotnet test` com cobertura executa sem erros

