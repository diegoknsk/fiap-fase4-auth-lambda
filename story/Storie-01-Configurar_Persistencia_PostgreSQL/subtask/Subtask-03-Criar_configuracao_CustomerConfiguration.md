# Subtask 04: Criar configuração de mapeamento CustomerConfiguration

## Descrição
Criar classe de configuração CustomerConfiguration que mapeia a entidade Customer do Domain para a tabela Customers no banco de dados, incluindo mapeamento de Value Objects como strings.

## Passos de implementação
- Criar diretório `src/FastFood.Auth.Infra.Persistence/Configurations/` se não existir
- Criar arquivo `src/FastFood.Auth.Infra.Persistence/Configurations/CustomerConfiguration.cs`
- Criar classe `CustomerConfiguration` implementando `IEntityTypeConfiguration<CustomerEntity>` (usar CustomerEntity, não Customer do Domain)
- Configurar tabela como "Customers" usando `ToTable("Customers")`
- Configurar chave primária: `HasKey(c => c.Id)`
- Configurar propriedades:
  - `Name`: HasMaxLength(500), IsRequired(false)
  - `Email`: HasMaxLength(255), IsRequired(false) (já é string, não precisa conversão)
  - `Cpf`: HasMaxLength(11), IsRequired(false) (já é string, não precisa conversão)
  - `CustomerType`: IsRequired(true) (já é int)
  - `CreatedAt`: IsRequired(true)
- No AuthDbContext, adicionar `modelBuilder.ApplyConfiguration(new CustomerConfiguration())` no OnModelCreating
- **Nota:** CustomerEntity já tem propriedades como strings, então não precisa conversão de Value Objects (isso será feito no repositório)

## Como testar
- Executar `dotnet build` no projeto Infra.Persistence (deve compilar sem erros)
- Verificar que não há erros de referência
- Validar sintaxe das configurações de mapeamento

## Critérios de aceite
- Arquivo CustomerConfiguration.cs criado em `Configurations/`
- Classe implementa IEntityTypeConfiguration<CustomerEntity> (usando entidade de persistência)
- Tabela configurada como "Customers"
- Chave primária Id configurada
- Propriedades Name, Email, Cpf, CustomerType, CreatedAt configuradas corretamente
- Email e Cpf configurados com HasMaxLength (já são strings em CustomerEntity)
- Configuração aplicada no OnModelCreating do AuthDbContext
- Código compila sem erros

