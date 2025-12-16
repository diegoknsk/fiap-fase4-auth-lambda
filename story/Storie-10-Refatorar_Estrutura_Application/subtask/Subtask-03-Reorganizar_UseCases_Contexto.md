# Subtask 03: Reorganizar UseCases por contexto e atualizar referências

## Descrição
Verificar que UseCases já estão organizados por contexto e atualizar todas as referências para usar InputModels e OutputModels ao invés de Commands e Responses.

## Passos de implementação
- Verificar que UseCases já estão em `UseCases/Customer/` e `UseCases/Admin/` (já está correto)
- Atualizar `UseCases/Customer/CreateAnonymousCustomerUseCase.cs`:
  - Trocar `using FastFood.Auth.Application.Responses.Customer;` → `using FastFood.Auth.Application.OutputModels.Customer;`
  - Trocar `CreateAnonymousCustomerResponse` → `CreateAnonymousCustomerOutputModel`
  - Atualizar tipo de retorno do método `ExecuteAsync()` para `CreateAnonymousCustomerOutputModel`
- Atualizar `UseCases/Customer/RegisterCustomerUseCase.cs`:
  - Trocar `using FastFood.Auth.Application.Commands.Customer;` → `using FastFood.Auth.Application.InputModels.Customer;`
  - Trocar `using FastFood.Auth.Application.Responses.Customer;` → `using FastFood.Auth.Application.OutputModels.Customer;`
  - Trocar `RegisterCustomerCommand` → `RegisterCustomerInputModel`
  - Trocar `RegisterCustomerResponse` → `RegisterCustomerOutputModel`
  - Atualizar assinatura do método `ExecuteAsync(RegisterCustomerInputModel inputModel)`
  - Atualizar tipo de retorno para `RegisterCustomerOutputModel`
- Atualizar `UseCases/Customer/IdentifyCustomerUseCase.cs`:
  - Trocar `using FastFood.Auth.Application.Commands.Customer;` → `using FastFood.Auth.Application.InputModels.Customer;`
  - Trocar `using FastFood.Auth.Application.Responses.Customer;` → `using FastFood.Auth.Application.OutputModels.Customer;`
  - Trocar `IdentifyCustomerCommand` → `IdentifyCustomerInputModel`
  - Trocar `IdentifyCustomerResponse` → `IdentifyCustomerOutputModel`
  - Atualizar assinatura do método `ExecuteAsync(IdentifyCustomerInputModel inputModel)`
  - Atualizar tipo de retorno para `IdentifyCustomerOutputModel`
- Atualizar `UseCases/Admin/AuthenticateAdminUseCase.cs`:
  - Trocar `using FastFood.Auth.Application.Commands.Admin;` → `using FastFood.Auth.Application.InputModels.Admin;`
  - Trocar `using FastFood.Auth.Application.Responses.Admin;` → `using FastFood.Auth.Application.OutputModels.Admin;`
  - Trocar `AuthenticateAdminCommand` → `AuthenticateAdminInputModel`
  - Trocar `AuthenticateAdminResponse` → `AuthenticateAdminOutputModel`
  - Atualizar assinatura do método `ExecuteAsync(AuthenticateAdminInputModel inputModel)`
  - Atualizar tipo de retorno para `AuthenticateAdminOutputModel`
- Atualizar instanciação de objetos dentro dos UseCases (trocar `new RegisterCustomerResponse` → `new RegisterCustomerOutputModel`, etc.)

## Como testar
- Executar `dotnet build` no projeto Application (deve compilar sem erros)
- Verificar que todos os `using` foram atualizados corretamente
- Verificar que todas as referências a Commands foram trocadas para InputModels
- Verificar que todas as referências a Responses foram trocadas para OutputModels
- Executar busca por "Command" e "Response" no projeto Application (não deve encontrar referências antigas)

## Critérios de aceite
- Todos os UseCases atualizados para usar InputModels e OutputModels
- Namespaces atualizados corretamente
- Assinaturas de métodos atualizadas
- Instanciação de objetos atualizada
- Projeto compila sem erros

