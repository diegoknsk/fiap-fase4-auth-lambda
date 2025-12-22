# Subtask 03: Adicionar endpoint identify no CustomerController

## Descrição
Adicionar endpoint POST `/customer/identify` no CustomerController que recebe CPF e chama o UseCase IdentifyCustomerUseCase.

## Passos de implementação
- Abrir arquivo `src/FastFood.Auth.Lambda/Controllers/CustomerController.cs`
- Adicionar campo privado `_identifyUseCase` do tipo `IdentifyCustomerUseCase` (injetado via construtor)
- Criar método `[HttpPost("identify")]` que:
  - Recebe `IdentifyCustomerRequest` do body
  - Mapeia para `IdentifyCustomerCommand`
  - Chama `await _identifyUseCase.ExecuteAsync(command)`
  - Retorna `Ok(result)` ou `Unauthorized` se exceção
- Criar classe `IdentifyCustomerRequest` em `Models/Customer/` com propriedade `Cpf`
- Registrar `IdentifyCustomerUseCase` no Program.cs

## Como testar
- Executar `dotnet build` (deve compilar sem erros)
- Executar aplicação e verificar endpoint no Swagger
- Testar endpoint com CPF existente e não existente

## Critérios de aceite
- Endpoint POST `/customer/identify` criado
- Endpoint retorna 200 com token quando customer existe
- Endpoint retorna 401 quando customer não existe
- Endpoint aparece no Swagger
- Código compila sem erros

