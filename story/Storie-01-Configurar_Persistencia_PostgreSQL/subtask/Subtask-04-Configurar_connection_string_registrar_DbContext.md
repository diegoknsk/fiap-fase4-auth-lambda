# Subtask 05: Configurar connection string e registrar DbContext

## Descrição
Configurar a connection string do PostgreSQL no appsettings.json e registrar o DbContext no container de injeção de dependência do Program.cs.

## Passos de implementação
- Abrir arquivo `src/FastFood.Auth.Lambda/appsettings.json`
- Adicionar seção "ConnectionStrings" com chave "DefaultConnection" e valor placeholder (ex: "Host=localhost;Port=5432;Database=fastfood_auth;Username=postgres;Password=postgres")
- Abrir arquivo `src/FastFood.Auth.Lambda/Program.cs`
- Adicionar using: `Microsoft.EntityFrameworkCore`, `FastFood.Auth.Infra.Persistence`
- Adicionar referência de projeto ao Infra.Persistence no .csproj do Lambda (se não existir)
- No `builder.Services`, adicionar `AddDbContext<AuthDbContext>` configurando para usar PostgreSQL com connection string do appsettings
- Usar `builder.Configuration.GetConnectionString("DefaultConnection")` para obter a connection string

## Como testar
- Executar `dotnet build` na solução (deve compilar sem erros)
- Verificar que o DbContext está registrado no container DI
- Validar que a connection string está sendo lida corretamente da configuração

## Critérios de aceite
- Connection string configurada em appsettings.json na seção ConnectionStrings
- DbContext registrado no Program.cs usando AddDbContext
- Connection string obtida via GetConnectionString
- PostgreSQL configurado como provider do EF Core
- Referência de projeto adicionada ao Infra.Persistence no Lambda
- Código compila sem erros

