# Storie-09: Implementar Presenters para Adaptação de Dados

## Descrição
Como desenvolvedor, quero implementar presenters na camada Application para fazer a adaptação dos dados de resposta dos UseCases, para que a camada de apresentação esteja desacoplada e siga os princípios da Clean Architecture.

## Objetivo
Refatorar a implementação atual movendo presenters e ResponseModels para a camada Application, seguindo o padrão arquitetural onde a Application define o contrato de saída e a API apenas consome esse contrato.

## Contexto
Atualmente, os presenters e ResponseModels estão na camada Lambda (API), mas segundo os princípios da Clean Architecture, faz mais sentido que:
- ResponseModels fiquem na Application (já existem como Application Responses)
- Presenters fiquem na Application para fazer transformações quando necessário
- API apenas chama UseCases e retorna os resultados
- Isso elimina duplicação e centraliza o contrato na Application

## Escopo Técnico
- Tecnologias: .NET 8, ASP.NET Core
- Arquivos afetados:
  - `src/FastFood.Auth.Application/Presenters/Customer/RegisterCustomerPresenter.cs` (mover da Lambda)
  - `src/FastFood.Auth.Application/Presenters/Customer/IdentifyCustomerPresenter.cs` (mover da Lambda)
  - `src/FastFood.Auth.Application/Presenters/Customer/CreateAnonymousCustomerPresenter.cs` (mover da Lambda)
  - `src/FastFood.Auth.Application/Presenters/Admin/AuthenticateAdminPresenter.cs` (mover da Lambda)
  - `src/FastFood.Auth.Lambda/Models/Customer/*.cs` (remover - usar Application Responses)
  - `src/FastFood.Auth.Lambda/Models/Admin/*.cs` (remover - usar Application Responses)
  - `src/FastFood.Auth.Lambda/Presenters/**/*.cs` (remover - mover para Application)
  - `src/FastFood.Auth.Lambda/Controllers/CustomerController.cs` (atualizar para usar presenters da Application)
  - `src/FastFood.Auth.Lambda/Controllers/AdminController.cs` (atualizar para usar presenters da Application)
  - `src/FastFood.Auth.Lambda/Program.cs` (atualizar registro de presenters)
  - `rules/baseprojectcontext.mdc` (atualizar regra sobre localização de presenters)

## Subtasks

- [Subtask 01: Mover Presenters para Application](./subtask/Subtask-01-Mover_Presenters_Application.md)
- [Subtask 02: Remover ResponseModels duplicados da Lambda](./subtask/Subtask-02-Remover_ResponseModels_Lambda.md)
- [Subtask 03: Atualizar Controllers para usar Presenters da Application](./subtask/Subtask-03-Atualizar_Controllers_Application.md)
- [Subtask 04: Atualizar Rules para documentar padrão](./subtask/Subtask-04-Atualizar_Rules.md)

## Critérios de Aceite da História

- [ ] Presenters movidos para a camada Application
- [ ] ResponseModels duplicados removidos da Lambda
- [ ] Controllers atualizados para usar presenters da Application
- [ ] Application define o contrato de saída (ResponseModels)
- [ ] API apenas consome o contrato da Application
- [ ] Código compila sem erros
- [ ] Testes atualizados e passando
- [ ] Endpoints continuam funcionando corretamente
- [ ] Rules atualizadas com o padrão arquitetural
- [ ] Sem violações críticas de Sonar

