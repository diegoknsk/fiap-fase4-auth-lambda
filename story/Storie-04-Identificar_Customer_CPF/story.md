# Storie-04: Identificar Customer por CPF

## Descrição
Como cliente, quero identificar um customer existente através do CPF, para que eu possa obter um token de autenticação válido sem precisar me registrar novamente.

## Objetivo
Implementar o endpoint `/customer/identify` que permite identificar um customer existente através do CPF e retornar um token JWT válido para autenticação. O sistema deve validar o CPF, buscar o customer no banco de dados PostgreSQL e gerar um token com as regras estabelecidas.

## Escopo Técnico
- Tecnologias: .NET 8, ASP.NET Core, Entity Framework Core, PostgreSQL, JWT
- Arquivos afetados:
  - `src/FastFood.Auth.Application/UseCases/Customer/IdentifyCustomerUseCase.cs`
  - `src/FastFood.Auth.Application/Commands/Customer/IdentifyCustomerCommand.cs`
  - `src/FastFood.Auth.Application/Responses/Customer/IdentifyCustomerResponse.cs`
  - `src/FastFood.Auth.Lambda/Controllers/CustomerController.cs` (adicionar endpoint)
  - `src/FastFood.Auth.Lambda/Models/Customer/IdentifyCustomerRequest.cs`
- Recursos: Endpoint POST `/customer/identify` que identifica customer e retorna token

## Subtasks

- [Subtask 01: Criar Command IdentifyCustomerCommand](./subtask/Subtask-01-Criar_Command_IdentifyCustomerCommand.md)
- [Subtask 02: Criar UseCase IdentifyCustomerUseCase](./subtask/Subtask-02-Criar_UseCase_IdentifyCustomer.md)
- [Subtask 03: Adicionar endpoint identify no CustomerController](./subtask/Subtask-03-Adicionar_endpoint_identify_Controller.md)
- [Subtask 04: Criar testes unitários para IdentifyCustomerUseCase](./subtask/Subtask-04-Criar_testes_unitarios_IdentifyCustomer.md)

## Critérios de Aceite da História

- [ ] Command IdentifyCustomerCommand criado com propriedade Cpf
- [ ] UseCase IdentifyCustomerUseCase criado e funcionando
- [ ] UseCase valida CPF usando Value Object Cpf do Domain
- [ ] UseCase busca customer pelo CPF no repositório
- [ ] Se customer não encontrado, retorna erro 401 (Unauthorized)
- [ ] Se customer encontrado, gera token JWT e retorna
- [ ] Endpoint POST `/customer/identify` criado no CustomerController
- [ ] Endpoint retorna token JWT válido quando customer existe
- [ ] Endpoint retorna 401 quando customer não existe
- [ ] Testes unitários criados e passando
- [ ] Endpoint documentado no Swagger
- [ ] Código compila sem erros
- [ ] Sem violações críticas de Sonar

