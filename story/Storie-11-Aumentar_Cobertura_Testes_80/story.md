# Storie-11: Aumentar Cobertura de Testes para 80%

## Descrição
Como desenvolvedor, quero aumentar a cobertura de testes unitários de 43% para pelo menos 80%, garantindo que todas as camadas (Domain, Application, Infra e Lambda) tenham testes adequados para validar regras de negócio, fluxos principais e tratamento de erros.

## Objetivo
Implementar suite completa de testes unitários cobrindo todos os componentes críticos do sistema, focando especialmente em:
- Serviços de infraestrutura (CognitoService, TokenService, CustomerRepository)
- Casos de borda e tratamento de erros nos UseCases
- Validações e cenários alternativos nos Controllers
- Correção de testes existentes sem assertions
- Cobertura completa de exceções e fluxos de erro

## Situação Atual
- Cobertura atual: **43%**
- Meta: **>= 80%**
- Issues do SonarQube: 22 issues identificados
- Testes sem assertions: 8 casos de teste em DomainValidationTests.cs
- Componentes sem cobertura: CognitoService, TokenService, CustomerRepository

## Escopo Técnico
- Tecnologias: .NET 8, xUnit, Moq, coverlet.collector
- Arquivos afetados:
  - `src/tests/FastFood.Auth.Tests.Unit/` (todos os testes)
  - `src/FastFood.Auth.Infra/Services/` (CognitoService, TokenService)
  - `src/FastFood.Auth.Infra.Persistence/Repositories/` (CustomerRepository)
  - `src/FastFood.Auth.Application/UseCases/` (cenários adicionais)
  - `src/FastFood.Auth.Lambda/Controllers/` (cenários adicionais)
- Recursos: Suite de testes unitários com cobertura >= 80%

## Priorização
1. **Alta Prioridade**: Infra Services (CognitoService, TokenService) - 0% de cobertura
2. **Alta Prioridade**: Repository (CustomerRepository) - 0% de cobertura
3. **Média Prioridade**: Correção de testes sem assertions (DomainValidationTests)
4. **Média Prioridade**: Cenários adicionais em UseCases (tratamento de erros)
5. **Média Prioridade**: Cenários adicionais em Controllers (validações e erros)
6. **Baixa Prioridade**: Testes de InputModels/OutputModels (se necessário)

## Subtasks

- [Subtask 01: Corrigir testes sem assertions em DomainValidationTests](./subtask/Subtask-01-Corrigir_testes_sem_assertions.md)
- [Subtask 02: Criar testes unitários para CognitoService](./subtask/Subtask-02-Testes_CognitoService.md)
- [Subtask 03: Criar testes unitários para TokenService](./subtask/Subtask-03-Testes_TokenService.md)
- [Subtask 04: Criar testes unitários para CustomerRepository](./subtask/Subtask-04-Testes_CustomerRepository.md)
- [Subtask 05: Adicionar cenários de erro nos UseCases de Customer](./subtask/Subtask-05-Cenarios_erro_UseCases_Customer.md)
- [Subtask 06: Adicionar cenários de erro no UseCase de Admin](./subtask/Subtask-06-Cenarios_erro_UseCase_Admin.md)
- [Subtask 07: Adicionar testes de validação e erro nos Controllers](./subtask/Subtask-07-Testes_validacao_Controllers.md)
- [Subtask 08: Adicionar testes para DomainException](./subtask/Subtask-08-Testes_DomainException.md)
- [Subtask 09: Adicionar cenários de borda em Value Objects](./subtask/Subtask-09-Cenarios_borda_ValueObjects.md)
- [Subtask 10: Adicionar cenários adicionais na entidade Customer](./subtask/Subtask-10-Cenarios_adicionais_Customer.md)
- [Subtask 11: Validar cobertura >= 80% e gerar relatório](./subtask/Subtask-11-Validar_cobertura_80.md)

## Critérios de Aceite da História

- [ ] Todos os testes sem assertions foram corrigidos
- [ ] CognitoService tem cobertura >= 80% com testes de:
  - Autenticação bem-sucedida
  - Tratamento de exceções do Cognito (NotAuthorizedException, UserNotFoundException, InvalidPasswordException)
  - Configuração inválida (Region, UserPoolId, ClientId ausentes)
  - Credenciais AWS expiradas
- [ ] TokenService tem cobertura >= 80% com testes de:
  - Geração de token válido
  - Configuração inválida (Secret, Issuer, Audience ausentes)
  - Diferentes valores de ExpirationHours
  - Validação de claims no token gerado
- [ ] CustomerRepository tem cobertura >= 80% com testes de:
  - GetByIdAsync (encontrado e não encontrado)
  - GetByCpfAsync (encontrado e não encontrado)
  - ExistsByCpfAsync (true e false)
  - AddAsync com mapeamento correto
  - Mapeamento Domain <-> Entity (com e sem Email/CPF)
- [ ] UseCases têm testes adicionais para:
  - Tratamento de exceções de repositório
  - Validações de entrada
  - Fluxos alternativos (customer existente vs novo)
- [ ] Controllers têm testes adicionais para:
  - Validação de ModelState
  - Tratamento de diferentes tipos de exceções
  - Retorno correto de status codes
- [ ] DomainException tem testes completos
- [ ] Value Objects têm testes de borda (valores limite, formatos especiais)
- [ ] Entidade Customer tem testes de todos os métodos e cenários
- [ ] Cobertura total >= 80% validada com coverlet
- [ ] Relatório de cobertura gerado e documentado
- [ ] Todos os testes passam no CI/CD

## Métricas de Sucesso

- **Cobertura de linha >= 80%** (medida com coverlet)
- **Cobertura por camada:**
  - Domain: >= 85%
  - Application: >= 80%
  - Infra: >= 75%
  - Lambda (Controllers): >= 70%
- **Zero testes sem assertions**
- **Todos os testes passando no CI/CD**

## Notas Técnicas

- Usar Moq para mockar dependências externas (AWS SDK, EF Core, IConfiguration)
- Testes de infraestrutura devem usar mocks para evitar dependências externas
- Focar em testes unitários puros (sem integração com banco ou AWS)
- Usar InMemory Database apenas se necessário para testes de repositório
- Manter testes rápidos e isolados

