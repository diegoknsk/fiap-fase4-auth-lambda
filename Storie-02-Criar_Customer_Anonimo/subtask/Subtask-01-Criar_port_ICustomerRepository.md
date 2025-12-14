# Subtask 01: Criar port ICustomerRepository na Application

## Descrição
Criar interface ICustomerRepository na camada Application que define os contratos para persistência de Customer, seguindo o padrão de ports da Clean Architecture.

## Passos de implementação
- Criar diretório `src/FastFood.Auth.Application/Ports/` se não existir
- Criar arquivo `src/FastFood.Auth.Application/Ports/ICustomerRepository.cs`
- Criar interface `ICustomerRepository` com métodos:
  - `Task<Customer?> GetByIdAsync(Guid id)`
  - `Task<Customer?> GetByCpfAsync(string cpf)`
  - `Task<bool> ExistsByCpfAsync(string cpf)`
  - `Task<Customer> AddAsync(Customer customer)`
- Adicionar using para `FastFood.Auth.Domain.Entities.CustomerIdentification`

## Como testar
- Executar `dotnet build` no projeto Application (deve compilar sem erros)
- Verificar que a interface está acessível e bem definida
- Validar que não há referências a infraestrutura na interface

## Critérios de aceite
- Arquivo ICustomerRepository.cs criado em `Application/Ports/`
- Interface define métodos GetByIdAsync, GetByCpfAsync, ExistsByCpfAsync, AddAsync
- Métodos retornam Task apropriado
- Interface não referencia nenhuma dependência de infraestrutura
- Código compila sem erros

