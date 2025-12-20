# Subtask 02: Renomear Responses para OutputModels e reorganizar por contexto

## Descrição
Renomear todos os Responses para OutputModels e reorganizar a estrutura de `Responses/Admin/` e `Responses/Customer/` para `OutputModels/Admin/` e `OutputModels/Customer/`, atualizando namespaces e referências.

## Passos de implementação
- Criar nova estrutura de pastas: `src/FastFood.Auth.Application/OutputModels/Customer/` e `OutputModels/Admin/`
- Mover e renomear `Responses/Customer/CreateAnonymousCustomerResponse.cs` → `OutputModels/Customer/CreateAnonymousCustomerOutputModel.cs`
- Mover e renomear `Responses/Customer/RegisterCustomerResponse.cs` → `OutputModels/Customer/RegisterCustomerOutputModel.cs`
- Mover e renomear `Responses/Customer/IdentifyCustomerResponse.cs` → `OutputModels/Customer/IdentifyCustomerOutputModel.cs`
- Mover e renomear `Responses/Admin/AuthenticateAdminResponse.cs` → `OutputModels/Admin/AuthenticateAdminOutputModel.cs`
- Atualizar namespaces em todos os arquivos renomeados:
  - `FastFood.Auth.Application.Responses.Customer` → `FastFood.Auth.Application.OutputModels.Customer`
  - `FastFood.Auth.Application.Responses.Admin` → `FastFood.Auth.Application.OutputModels.Admin`
- Atualizar nomes de classes:
  - `CreateAnonymousCustomerResponse` → `CreateAnonymousCustomerOutputModel`
  - `RegisterCustomerResponse` → `RegisterCustomerOutputModel`
  - `IdentifyCustomerResponse` → `IdentifyCustomerOutputModel`
  - `AuthenticateAdminResponse` → `AuthenticateAdminOutputModel`
- Deletar pasta `Responses/` após migração completa
- Atualizar referências nos UseCases e Presenters (será feito nas próximas subtasks, mas verificar que compila)

## Como testar
- Executar `dotnet build` no projeto Application (deve compilar sem erros)
- Verificar que os arquivos foram movidos para `OutputModels/Customer/` e `OutputModels/Admin/`
- Verificar que os namespaces foram atualizados corretamente
- Verificar que a pasta `Responses/` foi removida
- Executar busca por "Response" no projeto Application (não deve encontrar referências antigas de Application.Responses)

## Critérios de aceite
- Estrutura `OutputModels/Customer/` e `OutputModels/Admin/` criada
- Todos os Responses renomeados para OutputModels
- Namespaces atualizados corretamente
- Pasta `Responses/` removida
- Projeto compila sem erros







