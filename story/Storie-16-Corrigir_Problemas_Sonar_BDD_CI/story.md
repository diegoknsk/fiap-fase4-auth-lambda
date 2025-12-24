# Storie-16: Corrigir Problemas SonarQube, BDD Test e GitHub Actions

## Descrição
Como desenvolvedor, quero corrigir os problemas críticos identificados no SonarQube (Security Hotspots, cobertura e duplicação), o teste BDD que está falhando (identificar customer existente retornando 401), e os workflows do GitHub Actions que usam tags de versão ao invés de commit SHA hash completo, garantindo qualidade, segurança e conformidade com as melhores práticas.

## Objetivo
Resolver todos os problemas de qualidade e segurança identificados:
- **SonarQube**: 19 Security Hotspots, cobertura 63.7% (meta ≥80%), duplicação 4.8% (meta ≤3%)
- **BDD Test**: Corrigir teste "Identificar customer existente" que retorna 401 ao invés de 200
- **GitHub Actions**: Substituir todas as tags de versão (@v4, @v2, @v3) por commit SHA hash completo

## Situação Atual

### SonarQube
- **19 Security Hotspots** identificados
- **Cobertura**: 63.7% (requerido ≥ 80%) - **-16.3% abaixo da meta**
- **Duplicação**: 4.8% (requerido ≤ 3%) - **+1.8% acima do limite**

### BDD Test
- Teste "Identificar customer existente por CPF" está falhando:
  - Esperado: Status 200 OK
  - Atual: Status 401 Unauthorized
  - Erro: `Assert.Equal() Failure: Values differ Expected: OK Actual: Unauthorized`
  - Impacto: Teste crítico de autenticação não está validando corretamente

### GitHub Actions
- Múltiplos workflows usando tags de versão ao invés de commit SHA:
  - `aws-actions/configure-aws-credentials@v4`
  - `aws-actions/amazon-ecr-login@v2`
  - `docker/setup-buildx-action@v3`
  - `actions/checkout@v4`
  - `actions/setup-dotnet@v4`
  - `actions/upload-artifact@v4`
  - `actions/download-artifact@v4`
  - `actions/cache@v4`
  - `hashicorp/setup-terraform@v3`
  - `docker/build-push-action@v5`
  - E outros...

## Escopo Técnico
- Tecnologias: .NET 8, xUnit, SpecFlow, GitHub Actions, SonarQube
- Arquivos afetados:
  - `.github/workflows/*.yml` (todos os workflows)
  - `src/tests/FastFood.Auth.Tests.Bdd/` (testes BDD)
  - `src/Core/FastFood.Auth.Application/UseCases/Customer/IdentifyCustomerUseCase.cs`
  - `src/Core/FastFood.Auth.Infra.Persistence/Repositories/CustomerRepository.cs`
  - Código fonte com Security Hotspots
  - Código com duplicação
  - Código sem cobertura adequada

## Priorização
1. **Crítica**: Corrigir teste BDD falhando (bloqueia validação de funcionalidade)
2. **Alta**: Resolver Security Hotspots (risco de segurança)
3. **Alta**: Aumentar cobertura para ≥80% (qualidade)
4. **Média**: Reduzir duplicação para ≤3% (manutenibilidade)
5. **Média**: Atualizar GitHub Actions para usar SHA (segurança e reprodutibilidade)

## Subtasks

- [Subtask 01: Corrigir teste BDD "Identificar customer existente"](./subtask/Subtask-01-Corrigir_BDD_identify_customer.md)
- [Subtask 02: Resolver Security Hotspots do SonarQube](./subtask/Subtask-02-Resolver_Security_Hotspots.md)
- [Subtask 03: Aumentar cobertura de testes para ≥80%](./subtask/Subtask-03-Aumentar_cobertura_80.md)
- [Subtask 04: Reduzir duplicação de código para ≤3%](./subtask/Subtask-04-Reduzir_duplicacao_3.md)
- [Subtask 05: Atualizar GitHub Actions para usar commit SHA](./subtask/Subtask-05-Atualizar_GitHub_Actions_SHA.md)
- [Subtask 06: Validar todas as correções no SonarQube](./subtask/Subtask-06-Validar_SonarQube.md)

## Critérios de Aceite da História

### SonarQube
- [ ] **Security Hotspots**: Todos os 19 hotspots resolvidos ou justificados
- [ ] **Cobertura**: ≥ 80% de cobertura no código novo (atualmente 63.7%)
- [ ] **Duplicação**: ≤ 3% de duplicação no código novo (atualmente 4.8%)
- [ ] **Quality Gate**: Passa no Quality Gate do SonarQube

### BDD Test
- [ ] Teste "Identificar customer existente por CPF" passa com status 200
- [ ] Teste valida que o token JWT é retornado corretamente
- [ ] Teste valida que o customerId correto é retornado
- [ ] Todos os testes BDD passam no CI/CD

### GitHub Actions
- [ ] Todos os workflows usam commit SHA hash completo ao invés de tags
- [ ] Workflows são reprodutíveis e seguros
- [ ] Documentação atualizada com justificativa das versões escolhidas
- [ ] Todos os workflows passam no CI/CD

## Métricas de Sucesso

- **SonarQube Quality Gate**: ✅ Passa
- **Security Hotspots**: 0 (ou justificados)
- **Cobertura**: ≥ 80% (atualmente 63.7%)
- **Duplicação**: ≤ 3% (atualmente 4.8%)
- **BDD Tests**: 100% passando (atualmente 5/6)
- **GitHub Actions**: 100% usando commit SHA

## Notas Técnicas

### Security Hotspots
- Analisar cada hotspot individualmente
- Implementar correções de segurança apropriadas
- Documentar justificativas para hotspots que não podem ser corrigidos

### Cobertura
- Focar em código novo primeiro
- Adicionar testes para cenários não cobertos
- Priorizar componentes críticos (UseCases, Services, Repositories)

### Duplicação
- Identificar código duplicado
- Extrair para métodos/classes reutilizáveis
- Manter DRY (Don't Repeat Yourself)

### BDD Test
- Investigar problema de contexto do banco de dados
- Verificar mapeamento de CPF entre Domain e Entity
- Garantir isolamento entre testes

### GitHub Actions
- Usar commit SHA completo (ex: `@ff717079ee2060e4bcee96c4779b553acc87447c`)
- Manter comentário com versão (ex: `# v4`)
- Documentar processo de atualização de versões

