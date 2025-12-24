# Subtask 02: Resolver Security Hotspots do SonarQube

## Descrição
Analisar e resolver os 19 Security Hotspots identificados pelo SonarQube, implementando correções de segurança apropriadas ou documentando justificativas quando a correção não for possível.

## Problema Identificado
- **19 Security Hotspots** reportados pelo SonarQube
- Hotspots são pontos no código que podem representar vulnerabilidades de segurança
- Cada hotspot precisa ser analisado e corrigido ou justificado

## Passos de Implementação

1. **Obter lista completa de Security Hotspots:**
   - Acessar SonarQube Cloud e exportar lista de hotspots
   - Identificar arquivos e linhas afetadas
   - Categorizar hotspots por tipo (SQL Injection, Hardcoded Secrets, etc.)

2. **Analisar cada hotspot:**
   - Ler o código do hotspot
   - Entender o contexto e o risco
   - Determinar se é um falso positivo ou um risco real
   - Priorizar hotspots críticos

3. **Implementar correções:**
   - **Hardcoded Secrets**: Mover para variáveis de ambiente ou secrets
   - **SQL Injection**: Usar parâmetros ou ORM corretamente
   - **Weak Cryptography**: Usar algoritmos seguros
   - **Insecure Deserialization**: Validar e sanitizar entrada
   - **Path Traversal**: Validar e sanitizar caminhos
   - **XSS**: Escapar saída adequadamente
   - **CSRF**: Implementar proteção CSRF
   - **Authentication/Authorization**: Verificar autenticação adequada

4. **Documentar justificativas:**
   - Para hotspots que não podem ser corrigidos, documentar o motivo
   - Adicionar comentários no código explicando a decisão
   - Registrar no SonarQube como "Won't Fix" com justificativa

5. **Validar correções:**
   - Executar análise do SonarQube novamente
   - Verificar que os hotspots foram resolvidos
   - Confirmar que não há regressões

## Arquivos Afetados

- Arquivos identificados pelo SonarQube com Security Hotspots
- Possivelmente:
  - Controllers (validação de entrada)
  - Services (tratamento de dados sensíveis)
  - Repositories (queries SQL)
  - Configuration (secrets e configurações)

## Como Testar

- Executar análise do SonarQube: `dotnet-sonarscanner`
- Verificar que o número de Security Hotspots foi reduzido
- Validar que os hotspots críticos foram resolvidos
- Confirmar que o Quality Gate passa

## Critérios de Aceite

- [ ] Todos os 19 Security Hotspots foram analisados
- [ ] Hotspots críticos foram corrigidos
- [ ] Hotspots não críticos foram corrigidos ou justificados
- [ ] Justificativas documentadas no código ou no SonarQube
- [ ] Análise do SonarQube mostra 0 hotspots críticos
- [ ] Quality Gate do SonarQube passa

## Notas

- Priorizar hotspots que podem causar vulnerabilidades reais
- Alguns hotspots podem ser falsos positivos (ex: uso de `DateTime.Now` em contexto seguro)
- Documentar todas as decisões de segurança
- Considerar usar ferramentas adicionais de análise de segurança se necessário

