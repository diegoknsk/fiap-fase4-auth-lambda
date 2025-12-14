# Subtask 03: Adicionar endpoint register no CustomerController

## Descrição
Adicionar endpoint POST `/customer/register` no CustomerController que recebe CPF e chama o UseCase RegisterCustomerUseCase.

## Passos de implementação
- Abrir arquivo `src/FastFood.Auth.Lambda/Controllers/CustomerController.cs`
- Adicionar campo privado `_registerUseCase` do tipo `RegisterCustomerUseCase` (injetado via construtor)
- Criar método `[HttpPost("register")]` que:
  - Recebe `RegisterCustomerRequest` do body
  - Mapeia para `RegisterCustomerCommand`
  - Chama `await _registerUseCase.ExecuteAsync(command)`
  - Retorna `Ok(result)`
- Criar classe `RegisterCustomerRequest` em `Models/Customer/` com propriedade `Cpf`
- Registrar `RegisterCustomerUseCase` no Program.cs

## Como testar
- Executar `dotnet build` (deve compilar sem erros)
- Executar aplicação e verificar endpoint no Swagger
- Testar endpoint com CPF válido

## Critérios de aceite
- Endpoint POST `/customer/register` criado
- Endpoint recebe CPF no body
- Endpoint chama UseCase e retorna token
- Endpoint aparece no Swagger
- Código compila sem erros

