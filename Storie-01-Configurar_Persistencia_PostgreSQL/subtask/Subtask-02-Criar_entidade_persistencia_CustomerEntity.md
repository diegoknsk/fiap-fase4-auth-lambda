# Subtask 02: Criar entidade de persistência CustomerEntity

## Descrição
Criar classe CustomerEntity na camada Infra.Persistence que representa a entidade de banco de dados, separada da entidade de domínio Customer. Esta classe é similar ao CustomerModel do projeto anterior (Dapper), mas adaptada para EF Core.

## Passos de implementação
- Criar diretório `src/FastFood.Auth.Infra.Persistence/Entities/` se não existir
- Criar arquivo `src/FastFood.Auth.Infra.Persistence/Entities/CustomerEntity.cs`
- Criar classe `CustomerEntity` com propriedades:
  - `public Guid Id { get; set; }` - Chave primária
  - `public string? Name { get; set; }` - Nome (nullable, varchar 500)
  - `public string? Email { get; set; }` - Email (nullable, varchar 255) - armazenado como string
  - `public string? Cpf { get; set; }` - CPF (nullable, varchar 11) - armazenado como string
  - `public int CustomerType { get; set; }` - Tipo do customer (1 = Registered, 2 = Anonymous)
  - `public DateTime CreatedAt { get; set; }` - Data de criação
- Adicionar comentários explicando que esta é a entidade de persistência (separada do Domain)
- Nota: Esta classe representa a estrutura da tabela no banco, enquanto Customer (Domain) usa Value Objects (Cpf, Email)
- O repositório será responsável por fazer o mapeamento entre Customer (Domain) e CustomerEntity (Infra)

## Como testar
- Executar `dotnet build` no projeto Infra.Persistence (deve compilar sem erros)
- Verificar que a classe está acessível e bem definida
- Validar que todas as propriedades estão corretas

## Critérios de aceite
- Arquivo CustomerEntity.cs criado em `Infra.Persistence/Entities/`
- Classe tem propriedades: Id, Name, Email, Cpf, CustomerType, CreatedAt
- Propriedades Email e Cpf são strings (não Value Objects)
- Propriedades Name, Email, Cpf são nullable
- Propriedade CustomerType é int
- Propriedade CreatedAt é DateTime
- Código compila sem erros

