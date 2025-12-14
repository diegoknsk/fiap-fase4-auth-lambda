# Subtask 02: Criar UseCase RegisterCustomerUseCase

## Descrição
Criar UseCase RegisterCustomerUseCase que verifica se customer existe pelo CPF. Se existir, retorna token. Se não existir, cria novo customer Registered e retorna token.

## Passos de implementação
- Criar arquivo `src/FastFood.Auth.Application/UseCases/Customer/RegisterCustomerUseCase.cs`
- Criar classe `RegisterCustomerUseCase` recebendo `ICustomerRepository` e `ITokenService` via construtor
- Criar método `ExecuteAsync(RegisterCustomerCommand command)` que:
  - Valida CPF usando Value Object Cpf do Domain
  - Chama `_customerRepository.GetByCpfAsync(command.Cpf)`
  - Se customer existe: gera token e retorna
  - Se não existe: cria novo Customer com CustomerType.Registered, adiciona ao repository, gera token e retorna
- Criar classe `RegisterCustomerResponse` similar à CreateAnonymousCustomerResponse

## Como testar
- Executar `dotnet build` no projeto Application (deve compilar sem erros)
- Verificar que o UseCase está criado

## Critérios de aceite
- Arquivo RegisterCustomerUseCase.cs criado
- UseCase verifica se customer existe
- Se existe, retorna token do existente
- Se não existe, cria novo customer Registered
- Token gerado e retornado
- Código compila sem erros

