# Subtask 03: Criar DbContext AuthDbContext

## Descrição
Criar a classe AuthDbContext que herda de DbContext e configura o acesso ao banco de dados PostgreSQL, incluindo o DbSet para a entidade CustomerEntity (entidade de persistência).

## Passos de implementação
- Criar arquivo `src/FastFood.Auth.Infra.Persistence/AuthDbContext.cs`
- Criar classe `AuthDbContext` herdando de `DbContext`
- Adicionar propriedade `DbSet<CustomerEntity> Customers { get; set; }` (usar CustomerEntity, não Customer do Domain)
- Criar construtor que recebe `DbContextOptions<AuthDbContext>` e passa para base
- Adicionar método `OnModelCreating` para aplicar configurações (será usado na próxima subtask)
- Adicionar using statements necessários (Microsoft.EntityFrameworkCore, FastFood.Auth.Infra.Persistence.Entities)
- **Importante:** DbContext trabalha com CustomerEntity (entidade de persistência), não com Customer (entidade de domínio)

## Como testar
- Executar `dotnet build` no projeto Infra.Persistence (deve compilar sem erros)
- Verificar que não há erros de referência ou sintaxe
- Validar que a classe está acessível e herda corretamente de DbContext

## Critérios de aceite
- Arquivo AuthDbContext.cs criado em `src/FastFood.Auth.Infra.Persistence/`
- Classe AuthDbContext herda de DbContext
- Propriedade DbSet<CustomerEntity> Customers criada (usando entidade de persistência)
- Construtor recebe DbContextOptions<AuthDbContext>
- Método OnModelCreating criado (mesmo que vazio por enquanto)
- DbContext não referencia entidade Customer do Domain (apenas CustomerEntity da Infra)
- Código compila sem erros

