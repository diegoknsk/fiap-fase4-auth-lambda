# Storie-03: Registrar Customer

## Descrição
Como cliente, quero registrar um customer através do CPF, para que eu possa me identificar e receber um token de autenticação válido.

## Objetivo
Implementar o endpoint `/customer/register` que permite registrar um novo customer através do CPF. Se o customer já existir, deve retornar o token do customer existente. Se não existir, deve criar um novo customer com CustomerType = Registered (1) e retornar um token JWT válido.

## Escopo Técnico
- Tecnologias: .NET 8, ASP.NET Core, Entity Framework Core, PostgreSQL, JWT
- Arquivos afetados:
  - `src/FastFood.Auth.Application/UseCases/Customer/RegisterCustomerUseCase.cs`
  - `src/FastFood.Auth.Application/Commands/Customer/RegisterCustomerCommand.cs`
  - `src/FastFood.Auth.Application/Responses/Customer/RegisterCustomerResponse.cs`
  - `src/FastFood.Auth.Lambda/Controllers/CustomerController.cs` (adicionar endpoint)
  - `src/FastFood.Auth.Lambda/Models/Customer/RegisterCustomerRequest.cs`
- Recursos: Endpoint POST `/customer/register` que registra ou identifica customer e retorna token

## Subtasks

- [Subtask 01: Criar Command RegisterCustomerCommand](./subtask/Subtask-01-Criar_Command_RegisterCustomerCommand.md)
- [Subtask 02: Criar UseCase RegisterCustomerUseCase](./subtask/Subtask-02-Criar_UseCase_RegisterCustomer.md)
- [Subtask 03: Adicionar endpoint register no CustomerController](./subtask/Subtask-03-Adicionar_endpoint_register_Controller.md)
- [Subtask 04: Criar testes unitários para RegisterCustomerUseCase](./subtask/Subtask-04-Criar_testes_unitarios_RegisterCustomer.md)

## Critérios de Aceite da História

- [ ] Command RegisterCustomerCommand criado com propriedade Cpf
- [ ] UseCase RegisterCustomerUseCase criado e funcionando
- [ ] UseCase verifica se customer existe pelo CPF
- [ ] Se customer existe, retorna token do customer existente
- [ ] Se customer não existe, cria novo customer com CustomerType = Registered (1)
- [ ] Customer criado com CPF validado usando Value Object Cpf
- [ ] Endpoint POST `/customer/register` criado no CustomerController
- [ ] Endpoint retorna token JWT válido
- [ ] Testes unitários criados e passando
- [ ] Endpoint documentado no Swagger
- [ ] Código compila sem erros
- [ ] Sem violações críticas de Sonar

