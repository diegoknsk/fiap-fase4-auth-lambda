# Subtask 06: Criar e aplicar migration inicial

## Descrição
Criar a migration inicial para criar a tabela Customers no PostgreSQL. A migration será aplicada via GitHub Actions (ver Subtask 06), mas deve ser criada localmente primeiro para validação.

## Passos de implementação
- Abrir terminal no diretório raiz da solução
- Executar comando: `dotnet ef migrations add InitialCreate --project src/FastFood.Auth.Infra.Persistence --startup-project src/FastFood.Auth.Lambda`
- Verificar que a migration foi criada em `src/FastFood.Auth.Infra.Persistence/Migrations/`
- Verificar que o arquivo de migration contém criação da tabela Customers com todas as colunas corretas
- Validar localmente a migration (opcional, para desenvolvimento):
  - Configurar connection string local do PostgreSQL no appsettings.Development.json
  - Executar comando: `dotnet ef database update --project src/FastFood.Auth.Infra.Persistence --startup-project src/FastFood.Auth.Lambda`
  - Verificar no banco de dados local que a tabela Customers foi criada com estrutura correta
- **Nota:** A aplicação em produção será feita via GitHub Actions (Subtask 06)

## Como testar
- Executar `dotnet ef migrations list` para verificar que a migration aparece na lista
- Validar sintaxe do arquivo de migration gerado
- Se testar localmente: conectar ao PostgreSQL e executar `\d Customers` para verificar estrutura da tabela
- Verificar que todas as colunas estão presentes: Id, Name, Email, Cpf, CustomerType, CreatedAt
- Verificar tipos de dados: Id (uuid), Name (varchar 500), Email (varchar 255), Cpf (varchar 11), CustomerType (integer), CreatedAt (timestamp)

## Critérios de aceite
- Migration InitialCreate criada em `Migrations/`
- Migration contém criação da tabela Customers
- Estrutura da migration está correta (todas as colunas e tipos)
- Migration pode ser listada com `dotnet ef migrations list`
- Arquivo de migration compila sem erros
- (Opcional) Migration aplicada localmente para validação

