# Subtask 02: Criar UseCase IdentifyCustomerUseCase

## Descrição
Criar UseCase IdentifyCustomerUseCase que busca customer pelo CPF. Se encontrado, retorna token. Se não encontrado, lança exceção para retornar 401.

## Passos de implementação
- Criar arquivo `src/FastFood.Auth.Application/UseCases/Customer/IdentifyCustomerUseCase.cs`
- Criar classe `IdentifyCustomerUseCase` recebendo `ICustomerRepository` e `ITokenService` via construtor
- Criar método `ExecuteAsync(IdentifyCustomerCommand command)` que:
  - Valida CPF usando Value Object Cpf do Domain
  - Chama `_customerRepository.GetByCpfAsync(command.Cpf)`
  - Se customer não encontrado: lança `UnauthorizedAccessException`
  - Se encontrado: gera token e retorna resposta
- Criar classe `IdentifyCustomerResponse` similar às outras respostas

## Como testar
- Executar `dotnet build` no projeto Application (deve compilar sem erros)

## Critérios de aceite
- Arquivo IdentifyCustomerUseCase.cs criado
- UseCase busca customer pelo CPF
- Lança exceção se não encontrado
- Retorna token se encontrado
- Código compila sem erros

