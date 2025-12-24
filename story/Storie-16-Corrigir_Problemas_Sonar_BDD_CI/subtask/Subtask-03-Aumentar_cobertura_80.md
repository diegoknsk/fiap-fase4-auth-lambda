# Subtask 03: Aumentar cobertura de testes para ≥80%

## Descrição
Aumentar a cobertura de testes do código novo de 63.7% para pelo menos 80%, adicionando testes unitários para cenários não cobertos, focando especialmente em código novo adicionado recentemente.

## Problema Identificado
- **Cobertura atual**: 63.7%
- **Meta requerida**: ≥ 80%
- **Gap**: -16.3% abaixo da meta
- Código novo não está sendo adequadamente testado

## Passos de Implementação

1. **Identificar código não coberto:**
   - Executar análise de cobertura: `dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover`
   - Gerar relatório de cobertura
   - Identificar arquivos e métodos com baixa cobertura
   - Priorizar código novo adicionado recentemente

2. **Analisar gaps de cobertura:**
   - Identificar branches não cobertos (if/else, switch, etc.)
   - Identificar métodos não testados
   - Identificar exceções não testadas
   - Identificar casos de borda não cobertos

3. **Adicionar testes para código não coberto:**
   - **UseCases**: Adicionar testes para cenários alternativos e tratamento de erros
   - **Controllers**: Adicionar testes para validações e diferentes status codes
   - **Services**: Adicionar testes para todos os métodos e cenários
   - **Repositories**: Adicionar testes para todos os métodos
   - **Domain**: Adicionar testes para validações e casos de borda

4. **Focar em código novo:**
   - Identificar código adicionado nas últimas melhorias
   - Priorizar testes para esse código
   - Garantir que código novo tenha cobertura ≥ 80%

5. **Validar cobertura:**
   - Executar testes com cobertura novamente
   - Verificar que a cobertura está ≥ 80%
   - Confirmar que todos os testes passam

## Arquivos Afetados

- `src/tests/FastFood.Auth.Tests.Unit/` (adicionar novos testes)
- Arquivos com baixa cobertura identificados pela análise

## Como Testar

- Executar testes com cobertura:
  ```bash
  dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=./TestResults/coverage/
  ```
- Gerar relatório de cobertura
- Verificar que a cobertura está ≥ 80%
- Validar no SonarQube que a cobertura de código novo está ≥ 80%

## Critérios de Aceite

- [ ] Cobertura de código novo ≥ 80% (atualmente 63.7%)
- [ ] Todos os UseCases têm cobertura adequada
- [ ] Todos os Controllers têm cobertura adequada
- [ ] Todos os Services têm cobertura adequada
- [ ] Todos os Repositories têm cobertura adequada
- [ ] Casos de borda e tratamento de erros estão cobertos
- [ ] Relatório de cobertura gerado e documentado
- [ ] SonarQube valida cobertura ≥ 80%

## Notas

- Focar em código novo primeiro (últimas melhorias)
- Priorizar componentes críticos (UseCases, Services)
- Adicionar testes para cenários de erro e casos de borda
- Manter testes rápidos e isolados
- Usar mocks para dependências externas

