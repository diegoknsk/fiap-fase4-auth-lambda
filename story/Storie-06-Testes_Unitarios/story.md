# Storie-06: Implementar Testes Unitários

## Descrição
Como desenvolvedor, quero implementar testes unitários completos para Domain, UseCases e Controllers, para garantir qualidade do código e atingir cobertura mínima de 80%.

## Objetivo
Implementar suite completa de testes unitários usando xUnit e Moq, cobrindo entidades de domínio, value objects, use cases e controllers, garantindo que todas as regras de negócio e fluxos principais estejam testados.

## Escopo Técnico
- Tecnologias: .NET 8, xUnit, Moq, coverlet.collector
- Arquivos afetados:
  - `tests/FastFood.Auth.Tests.Unit/Domain/` (testes de entidades e value objects)
  - `tests/FastFood.Auth.Tests.Unit/UseCases/Customer/` (testes de use cases)
  - `tests/FastFood.Auth.Tests.Unit/UseCases/Admin/` (testes de use cases admin)
  - `tests/FastFood.Auth.Tests.Unit/Controllers/` (testes de controllers)
  - `tests/FastFood.Auth.Tests.Unit/FastFood.Auth.Tests.Unit.csproj` (adicionar Moq)
- Recursos: Suite de testes unitários com cobertura >= 80%

## Subtasks

- [Subtask 01: Adicionar pacote Moq ao projeto de testes](./subtask/Subtask-01-Adicionar_pacote_Moq.md)
- [Subtask 02: Criar testes unitários para Value Objects (Cpf, Email)](./subtask/Subtask-02-Criar_testes_ValueObjects.md)
- [Subtask 03: Criar testes unitários para entidade Customer](./subtask/Subtask-03-Criar_testes_entidade_Customer.md)
- [Subtask 04: Criar testes unitários para UseCases de Customer](./subtask/Subtask-04-Criar_testes_UseCases_Customer.md)
- [Subtask 05: Criar testes unitários para UseCase de Admin](./subtask/Subtask-05-Criar_testes_UseCase_Admin.md)
- [Subtask 06: Criar testes unitários para Controllers](./subtask/Subtask-06-Criar_testes_Controllers.md)
- [Subtask 07: Configurar cobertura de código e validar >= 80%](./subtask/Subtask-07-Configurar_cobertura_codigo.md)

## Critérios de Aceite da História

- [ ] Pacote Moq adicionado ao projeto de testes unitários
- [ ] Testes para Value Object Cpf criados (validação, formatação, casos inválidos)
- [ ] Testes para Value Object Email criados (validação, casos inválidos)
- [ ] Testes para entidade Customer criados (criação, validações de domínio)
- [ ] Testes para CreateAnonymousCustomerUseCase criados e passando
- [ ] Testes para RegisterCustomerUseCase criados e passando
- [ ] Testes para IdentifyCustomerUseCase criados e passando
- [ ] Testes para AuthenticateAdminUseCase criados e passando
- [ ] Testes para CustomerController criados e passando
- [ ] Testes para AdminController criados e passando
- [ ] Cobertura de código >= 80% validada
- [ ] Todos os testes passando (`dotnet test`)
- [ ] Testes executados no CI/CD (GitHub Actions)
- [ ] Código compila sem erros
- [ ] Sem violações críticas de Sonar

