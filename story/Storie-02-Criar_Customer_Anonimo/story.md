# Storie-02: Criar Customer Anônimo

## Descrição
Como cliente, quero criar um customer anônimo sem necessidade de identificação, para que eu possa continuar com o processo de compra mesmo sem me identificar.

## Objetivo
Implementar o endpoint `/customer/anonymous` que permite criar um customer anônimo (CustomerType = Anonymous) e retornar um token JWT válido para autenticação, sem necessidade de CPF ou outros dados pessoais.

## Escopo Técnico
- Tecnologias: .NET 8, ASP.NET Core, Entity Framework Core, PostgreSQL, JWT
- Arquivos afetados:
  - `src/FastFood.Auth.Application/UseCases/Customer/CreateAnonymousCustomerUseCase.cs`
  - `src/FastFood.Auth.Application/Commands/Customer/CreateAnonymousCustomerCommand.cs`
  - `src/FastFood.Auth.Application/Responses/Customer/CreateAnonymousCustomerResponse.cs`
  - `src/FastFood.Auth.Application/Ports/ICustomerRepository.cs`
  - `src/FastFood.Auth.Application/Ports/ITokenService.cs`
  - `src/FastFood.Auth.Infra.Persistence/Repositories/CustomerRepository.cs`
  - `src/FastFood.Auth.Infra/Services/TokenService.cs`
  - `src/FastFood.Auth.Lambda/Controllers/CustomerController.cs`
  - `src/FastFood.Auth.Lambda/Models/Customer/CreateAnonymousCustomerRequest.cs`
- Recursos: Endpoint POST `/customer/anonymous` que cria customer e retorna token

## Subtasks

- [Subtask 01: Criar port ICustomerRepository na Application](./subtask/Subtask-01-Criar_port_ICustomerRepository.md)
- [Subtask 02: Criar port ITokenService na Application](./subtask/Subtask-02-Criar_port_ITokenService.md)
- [Subtask 03: Implementar CustomerRepository na Infra.Persistence](./subtask/Subtask-03-Implementar_CustomerRepository.md)
- [Subtask 04: Implementar TokenService na Infra](./subtask/Subtask-04-Implementar_TokenService.md)
- [Subtask 05: Criar UseCase CreateAnonymousCustomerUseCase](./subtask/Subtask-05-Criar_UseCase_CreateAnonymousCustomer.md)
- [Subtask 06: Criar Controller CustomerController com endpoint anonymous](./subtask/Subtask-06-Criar_Controller_CustomerController.md)
- [Subtask 07: Registrar serviços no Program.cs](./subtask/Subtask-07-Registrar_servicos_Program.md)
- [Subtask 08: Criar testes unitários para UseCase](./subtask/Subtask-08-Criar_testes_unitarios_UseCase.md)
- [Subtask 09: Refatorar TokenService para Infra](./subtask/Subtask-09-Refatorar_TokenService_para_Infra.md)

## Critérios de Aceite da História

- [ ] Port ICustomerRepository criado na Application
- [ ] Port ITokenService criado na Application
- [ ] CustomerRepository implementado na Infra.Persistence usando EF Core
- [ ] TokenService implementado na Infra gerando JWT com claims corretas
- [ ] UseCase CreateAnonymousCustomerUseCase criado e funcionando
- [ ] Endpoint POST `/customer/anonymous` criado no CustomerController
- [ ] Endpoint retorna token JWT válido após criar customer anônimo
- [ ] Customer criado no banco com CustomerType = Anonymous (2)
- [ ] Customer criado com Id, Name=null, Email=null, Cpf=null, CreatedAt preenchido
- [ ] Serviços registrados corretamente no Program.cs
- [ ] Testes unitários criados e passando
- [ ] Endpoint documentado no Swagger
- [ ] Código compila sem erros
- [ ] Sem violações críticas de Sonar

