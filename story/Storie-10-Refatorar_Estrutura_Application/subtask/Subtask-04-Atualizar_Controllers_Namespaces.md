# Subtask 04: Atualizar Controllers para usar novos namespaces

## Descrição
Atualizar todos os Controllers (CustomerController, AdminController) para usar os novos namespaces de InputModels e OutputModels, e atualizar referências aos Presenters que agora usam OutputModels.

## Passos de implementação
- Atualizar `Controllers/CustomerController.cs`:
  - Trocar `using FastFood.Auth.Application.Commands.Customer;` → `using FastFood.Auth.Application.InputModels.Customer;`
  - Trocar `using FastFood.Auth.Application.Responses.Customer;` → `using FastFood.Auth.Application.OutputModels.Customer;`
  - Trocar `RegisterCustomerCommand` → `RegisterCustomerInputModel`
  - Trocar `IdentifyCustomerCommand` → `IdentifyCustomerInputModel`
  - Atualizar instanciação: `new RegisterCustomerCommand { Cpf = ... }` → `new RegisterCustomerInputModel { Cpf = ... }`
  - Atualizar instanciação: `new IdentifyCustomerCommand { Cpf = ... }` → `new IdentifyCustomerInputModel { Cpf = ... }`
  - Verificar que Presenters estão sendo usados corretamente (já devem estar usando OutputModels)
- Atualizar `Controllers/AdminController.cs` (se existir):
  - Trocar `using FastFood.Auth.Application.Commands.Admin;` → `using FastFood.Auth.Application.InputModels.Admin;`
  - Trocar `using FastFood.Auth.Application.Responses.Admin;` → `using FastFood.Auth.Application.OutputModels.Admin;`
  - Trocar `AuthenticateAdminCommand` → `AuthenticateAdminInputModel`
  - Atualizar instanciação: `new AuthenticateAdminCommand { ... }` → `new AuthenticateAdminInputModel { ... }`
- Atualizar Presenters (se necessário):
  - Verificar que `Presenters/Customer/*.cs` usam `OutputModels.Customer`
  - Verificar que `Presenters/Admin/*.cs` usam `OutputModels.Admin`
  - Atualizar assinaturas dos métodos `Present()` para receber `OutputModel` e retornar `ResponseModel` (da API)
- Verificar que ResponseModels da API (em `Lambda/Models/`) não foram alterados (são diferentes dos OutputModels)

## Como testar
- Executar `dotnet build` no projeto Lambda (deve compilar sem erros)
- Executar `dotnet run` e testar endpoints via Swagger:
  - `POST /api/customer/anonymous` deve funcionar
  - `POST /api/customer/register` deve funcionar
  - `POST /api/customer/identify` deve funcionar
  - `POST /api/admin/login` deve funcionar (se existir)
- Verificar que Swagger está funcionando corretamente
- Verificar que não há erros de compilação relacionados a namespaces

## Critérios de aceite
- Todos os Controllers atualizados para usar InputModels
- Todos os Controllers atualizados para usar OutputModels (via Presenters)
- Presenters atualizados para trabalhar com OutputModels
- Projeto Lambda compila sem erros
- Swagger funciona corretamente
- Endpoints respondem corretamente (teste manual via Swagger)



