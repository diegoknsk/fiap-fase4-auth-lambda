# Subtask 04: Reduzir duplicação de código para ≤3%

## Descrição
Reduzir a duplicação de código novo de 4.8% para no máximo 3%, identificando código duplicado e extraindo para métodos ou classes reutilizáveis, seguindo o princípio DRY (Don't Repeat Yourself).

## Problema Identificado
- **Duplicação atual**: 4.8%
- **Meta requerida**: ≤ 3%
- **Gap**: +1.8% acima do limite
- Código novo contém duplicação que pode ser refatorada

## Passos de Implementação

1. **Identificar código duplicado:**
   - Executar análise do SonarQube
   - Exportar relatório de duplicação
   - Identificar blocos de código duplicados
   - Priorizar duplicação em código novo

2. **Analisar duplicação:**
   - Identificar padrões de duplicação
   - Categorizar por tipo (lógica de negócio, validações, mapeamentos, etc.)
   - Determinar se a duplicação é justificada ou pode ser extraída

3. **Extrair código duplicado:**
   - **Métodos privados**: Extrair lógica comum para métodos privados
   - **Classes base**: Criar classes base para código compartilhado
   - **Helpers/Utils**: Criar classes utilitárias para código reutilizável
   - **Extension Methods**: Criar métodos de extensão quando apropriado
   - **Value Objects**: Usar Value Objects para encapsular lógica comum

4. **Refatorar código:**
   - Substituir código duplicado por chamadas aos métodos/classes extraídos
   - Garantir que a funcionalidade não foi alterada
   - Manter testes existentes passando

5. **Validar redução:**
   - Executar análise do SonarQube novamente
   - Verificar que a duplicação está ≤ 3%
   - Confirmar que não há regressões

## Arquivos Afetados

- Arquivos identificados pelo SonarQube com código duplicado
- Possivelmente:
  - Controllers (validações e mapeamentos)
  - UseCases (lógica comum)
  - Services (tratamento de dados)
  - Repositories (queries similares)

## Como Testar

- Executar análise do SonarQube
- Verificar que a duplicação está ≤ 3%
- Executar todos os testes: `dotnet test`
- Confirmar que não há regressões funcionais

## Critérios de Aceite

- [ ] Duplicação de código novo ≤ 3% (atualmente 4.8%)
- [ ] Código duplicado foi extraído para métodos/classes reutilizáveis
- [ ] Funcionalidade não foi alterada (testes passam)
- [ ] Código está mais manutenível e segue DRY
- [ ] SonarQube valida duplicação ≤ 3%

## Notas

- Alguma duplicação pode ser justificada (ex: validações específicas por contexto)
- Priorizar duplicação que afeta manutenibilidade
- Considerar trade-off entre duplicação e complexidade
- Documentar decisões quando duplicação é mantida intencionalmente

