# Storie-01: Configurar Persistência PostgreSQL e Migrations

## Descrição
Como desenvolvedor, quero configurar Entity Framework Core com PostgreSQL para persistência da entidade Customer, para que o sistema possa armazenar e recuperar dados de customers no banco de dados.

## Objetivo
Configurar Entity Framework Core com PostgreSQL, criar DbContext, configurações de mapeamento, migrations e garantir que a tabela Customers seja criada corretamente no banco de dados com a estrutura especificada.

## Escopo Técnico
- Tecnologias: .NET 8, Entity Framework Core, Npgsql.EntityFrameworkCore.PostgreSQL
- Arquivos afetados: 
  - `src/FastFood.Auth.Infra.Persistence/Entities/CustomerEntity.cs` (entidade de persistência, similar ao CustomerModel do projeto Dapper)
  - `src/FastFood.Auth.Infra.Persistence/AuthDbContext.cs`
  - `src/FastFood.Auth.Infra.Persistence/Configurations/CustomerConfiguration.cs`
  - `src/FastFood.Auth.Infra.Persistence/Migrations/`
  - `src/FastFood.Auth.Lambda/appsettings.json`
  - `src/FastFood.Auth.Infra.Persistence/FastFood.Auth.Infra.Persistence.csproj`
- Recursos: Tabela Customers no PostgreSQL com estrutura:
  - Id (Guid, PK)
  - Name (varchar 500, nullable)
  - Email (varchar 255, nullable)
  - Cpf (varchar 11, nullable)
  - CustomerType (int) - 1 = Registered, 2 = Anonymous
  - CreatedAt (datetime)

## Subtasks

- [Subtask 01: Adicionar pacote Npgsql.EntityFrameworkCore.PostgreSQL](./subtask/Subtask-01-Adicionar_pacote_Npgsql.md)
- [Subtask 02: Criar entidade de persistência CustomerEntity](./subtask/Subtask-02-Criar_entidade_persistencia_CustomerEntity.md)
- [Subtask 03: Criar DbContext AuthDbContext](./subtask/Subtask-03-Criar_DbContext_AuthDbContext.md)
- [Subtask 04: Criar configuração de mapeamento CustomerConfiguration](./subtask/Subtask-04-Criar_configuracao_CustomerConfiguration.md)
- [Subtask 05: Configurar connection string e registrar DbContext](./subtask/Subtask-05-Configurar_connection_string_registrar_DbContext.md)
- [Subtask 06: Criar e aplicar migration inicial](./subtask/Subtask-06-Criar_aplicar_migration_inicial.md)
- [Subtask 07: Criar workflow GitHub Actions para executar migrations](./subtask/Subtask-07-Criar_workflow_GitHub_Actions_migrations.md)

## Critérios de Aceite da História

- [ ] Pacote Npgsql.EntityFrameworkCore.PostgreSQL adicionado ao projeto Infra.Persistence
- [ ] Entidade de persistência CustomerEntity criada (separada da entidade de domínio Customer)
- [ ] CustomerEntity mapeia propriedades simples (strings) em vez de Value Objects
- [ ] DbContext AuthDbContext criado em `src/FastFood.Auth.Infra.Persistence/AuthDbContext.cs`
- [ ] Configuração CustomerConfiguration criada mapeando CustomerEntity para tabela Customers
- [ ] Value Objects Cpf e Email mapeados como strings no banco
- [ ] Connection string configurada em appsettings.json
- [ ] DbContext registrado no Program.cs com PostgreSQL
- [ ] Migration inicial criada e aplicada com sucesso
- [ ] Tabela Customers criada no PostgreSQL com estrutura correta (Id, Name, Email, Cpf, CustomerType, CreatedAt)
- [ ] Workflow GitHub Actions criado para executar migrations automaticamente
- [ ] Workflow configurado com secrets necessários (connection string)
- [ ] Código compila sem erros
- [ ] Sem violações críticas de Sonar

