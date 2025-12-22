# Storie-10: Refatorar Estrutura da Camada Application para Padrão Horizontal por Contexto

## Status
- **Estado:** ✅ Concluída
- **Data de Conclusão:** 20/12/2024

## Descrição
Como arquiteto de software, quero refatorar a estrutura da camada Application para seguir o padrão horizontal por contexto (organização por Customer/Admin dentro de cada pasta), para garantir consistência com a arquitetura alvo e facilitar manutenção e evolução do código.

## Objetivo
Reorganizar a estrutura da camada `FastFood.Auth.Application` de organização vertical (por tipo: Commands/, UseCases/, Responses/) para organização horizontal por contexto (Customer/, Admin/ dentro de cada pasta), renomeando Commands para InputModels e Responses para OutputModels, mantendo toda a lógica de negócio intacta.

## Escopo Técnico
- Tecnologias: .NET 8, C#
- Arquivos afetados:
  - `src/FastFood.Auth.Application/` (toda a estrutura)
  - `src/FastFood.Auth.Lambda/Controllers/` (ajustes de namespaces)
  - `tests/FastFood.Auth.Tests.Unit/` (ajustes de namespaces)
- Estrutura alvo:
  ```
  Application/
    UseCases/
      Customer/
      Admin/
    InputModels/
      Customer/
      Admin/
    OutputModels/
      Customer/
      Admin/
    Presenters/
      Customer/
      Admin/
    Ports/
  ```

## Subtasks

- [Subtask 01: Renomear Commands para InputModels e reorganizar por contexto](./subtask/Subtask-01-Renomear_Commands_InputModels.md)
- [Subtask 02: Renomear Responses para OutputModels e reorganizar por contexto](./subtask/Subtask-02-Renomear_Responses_OutputModels.md)
- [Subtask 03: Reorganizar UseCases por contexto (já está parcialmente correto)](./subtask/Subtask-03-Reorganizar_UseCases_Contexto.md)
- [Subtask 04: Atualizar Controllers para usar novos namespaces](./subtask/Subtask-04-Atualizar_Controllers_Namespaces.md)
- [Subtask 05: Atualizar testes unitários para novos namespaces](./subtask/Subtask-05-Atualizar_Testes_Namespaces.md)

## Critérios de Aceite da História

- [x] Todos os Commands foram renomeados para InputModels e organizados por contexto (Customer/, Admin/)
- [x] Todos os Responses foram renomeados para OutputModels e organizados por contexto (Customer/, Admin/)
- [x] UseCases estão organizados por contexto dentro de UseCases/
- [x] Presenters estão organizados por contexto dentro de Presenters/
- [x] Estrutura final segue o padrão horizontal por contexto conforme arquitetura alvo
- [x] Todos os Controllers foram atualizados para usar novos namespaces (InputModels, OutputModels)
- [x] Todos os testes unitários foram atualizados para usar novos namespaces
- [x] Projeto compila sem erros (`dotnet build`)
- [x] Todos os testes passam (`dotnet test`)
- [x] Swagger continua funcionando corretamente
- [x] Nenhuma regra de negócio foi alterada (apenas reorganização estrutural)
- [x] Namespaces seguem o padrão: `FastFood.Auth.Application.InputModels.Customer`, `FastFood.Auth.Application.OutputModels.Admin`, etc.

