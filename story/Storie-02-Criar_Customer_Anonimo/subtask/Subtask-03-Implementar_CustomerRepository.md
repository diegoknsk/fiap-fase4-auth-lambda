# Subtask 03: Implementar CustomerRepository na Infra.Persistence

## Descrição
Implementar classe CustomerRepository que implementa ICustomerRepository usando Entity Framework Core e AuthDbContext para persistência no PostgreSQL.

## Passos de implementação
- Criar diretório `src/FastFood.Auth.Infra.Persistence/Repositories/` se não existir
- Criar arquivo `src/FastFood.Auth.Infra.Persistence/Repositories/CustomerRepository.cs`
- Criar classe `CustomerRepository` implementando `ICustomerRepository`
- Adicionar campo privado `_context` do tipo `AuthDbContext` (injetado via construtor)
- Implementar método `GetByIdAsync`: usar `_context.Customers.FindAsync(id)`
- Implementar método `GetByCpfAsync`: usar `_context.Customers.FirstOrDefaultAsync(c => c.Cpf.Value == cpf)`
- Implementar método `ExistsByCpfAsync`: usar `_context.Customers.AnyAsync(c => c.Cpf.Value == cpf)`
- Implementar método `AddAsync`: usar `_context.Customers.AddAsync(customer)` e `SaveChangesAsync()`
- Adicionar usings necessários (Microsoft.EntityFrameworkCore, FastFood.Auth.Application.Ports, FastFood.Auth.Domain.Entities.CustomerIdentification)

## Como testar
- Executar `dotnet build` no projeto Infra.Persistence (deve compilar sem erros)
- Verificar que a classe implementa corretamente a interface ICustomerRepository
- Validar que todos os métodos estão implementados

## Critérios de aceite
- Arquivo CustomerRepository.cs criado em `Infra.Persistence/Repositories/`
- Classe implementa ICustomerRepository
- Construtor recebe AuthDbContext via DI
- Métodos GetByIdAsync, GetByCpfAsync, ExistsByCpfAsync, AddAsync implementados
- Métodos usam EF Core corretamente
- Código compila sem erros

