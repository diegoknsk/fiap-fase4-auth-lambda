# Subtask 05: Atualizar testes unitários para novos namespaces

## Descrição
Atualizar todos os testes unitários para usar os novos namespaces de InputModels e OutputModels, garantindo que todos os testes continuem passando após a refatoração.

## Passos de implementação
- Buscar todos os arquivos de teste que referenciam Commands:
  - Trocar `using FastFood.Auth.Application.Commands.Customer;` → `using FastFood.Auth.Application.InputModels.Customer;`
  - Trocar `using FastFood.Auth.Application.Commands.Admin;` → `using FastFood.Auth.Application.InputModels.Admin;`
  - Trocar `RegisterCustomerCommand` → `RegisterCustomerInputModel`
  - Trocar `IdentifyCustomerCommand` → `IdentifyCustomerInputModel`
  - Trocar `AuthenticateAdminCommand` → `AuthenticateAdminInputModel`
- Buscar todos os arquivos de teste que referenciam Responses:
  - Trocar `using FastFood.Auth.Application.Responses.Customer;` → `using FastFood.Auth.Application.OutputModels.Customer;`
  - Trocar `using FastFood.Auth.Application.Responses.Admin;` → `using FastFood.Auth.Application.OutputModels.Admin;`
  - Trocar `CreateAnonymousCustomerResponse` → `CreateAnonymousCustomerOutputModel`
  - Trocar `RegisterCustomerResponse` → `RegisterCustomerOutputModel`
  - Trocar `IdentifyCustomerResponse` → `IdentifyCustomerOutputModel`
  - Trocar `AuthenticateAdminResponse` → `AuthenticateAdminOutputModel`
- Atualizar instanciação de objetos nos testes:
  - `new RegisterCustomerCommand { ... }` → `new RegisterCustomerInputModel { ... }`
  - `new RegisterCustomerResponse { ... }` → `new RegisterCustomerOutputModel { ... }`
  - (e assim por diante para todos os tipos)
- Atualizar asserções que verificam tipos de retorno:
  - Verificar que asserções esperam `OutputModel` ao invés de `Response`
- Verificar que mocks de UseCases estão configurados corretamente:
  - Mocks devem retornar `OutputModels` ao invés de `Responses`

## Como testar
- Executar `dotnet build` no projeto de testes (deve compilar sem erros)
- Executar `dotnet test` (todos os testes devem passar)
- Verificar cobertura de código (deve manter >= 80%)
- Executar busca por "Command" e "Response" no projeto de testes (não deve encontrar referências antigas de Application.Commands ou Application.Responses)

## Critérios de aceite
- Todos os testes atualizados para usar InputModels
- Todos os testes atualizados para usar OutputModels
- Todos os testes passam (`dotnet test` retorna sucesso)
- Projeto de testes compila sem erros
- Cobertura de código mantida (>= 80%)
- Nenhuma referência antiga a Commands ou Responses encontrada









