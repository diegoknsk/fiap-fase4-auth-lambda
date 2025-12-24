# Subtask 01: Renomear Commands para InputModels e reorganizar por contexto

## Descrição
Renomear todos os Commands para InputModels e reorganizar a estrutura de `Commands/Admin/` e `Commands/Customer/` para `InputModels/Admin/` e `InputModels/Customer/`, atualizando namespaces e referências.

## Passos de implementação
- Criar nova estrutura de pastas: `src/FastFood.Auth.Application/InputModels/Customer/` e `InputModels/Admin/`
- Mover e renomear `Commands/Customer/RegisterCustomerCommand.cs` → `InputModels/Customer/RegisterCustomerInputModel.cs`
- Mover e renomear `Commands/Customer/IdentifyCustomerCommand.cs` → `InputModels/Customer/IdentifyCustomerInputModel.cs`
- Mover e renomear `Commands/Admin/AuthenticateAdminCommand.cs` → `InputModels/Admin/AuthenticateAdminInputModel.cs`
- Atualizar namespaces em todos os arquivos renomeados:
  - `FastFood.Auth.Application.Commands.Customer` → `FastFood.Auth.Application.InputModels.Customer`
  - `FastFood.Auth.Application.Commands.Admin` → `FastFood.Auth.Application.InputModels.Admin`
- Atualizar nomes de classes:
  - `RegisterCustomerCommand` → `RegisterCustomerInputModel`
  - `IdentifyCustomerCommand` → `IdentifyCustomerInputModel`
  - `AuthenticateAdminCommand` → `AuthenticateAdminInputModel`
- Deletar pasta `Commands/` após migração completa
- Atualizar referências nos UseCases (será feito na Subtask 03, mas verificar que compila)

## Como testar
- Executar `dotnet build` no projeto Application (deve compilar sem erros)
- Verificar que os arquivos foram movidos para `InputModels/Customer/` e `InputModels/Admin/`
- Verificar que os namespaces foram atualizados corretamente
- Verificar que a pasta `Commands/` foi removida
- Executar busca por "Command" no projeto Application (não deve encontrar referências antigas)

## Critérios de aceite
- Estrutura `InputModels/Customer/` e `InputModels/Admin/` criada
- Todos os Commands renomeados para InputModels
- Namespaces atualizados corretamente
- Pasta `Commands/` removida
- Projeto compila sem erros










