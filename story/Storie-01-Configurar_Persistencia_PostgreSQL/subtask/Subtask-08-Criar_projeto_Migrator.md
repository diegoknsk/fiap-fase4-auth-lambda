# Subtask 08: Criar projeto Migrator para executar migrations

## Descrição
Criar um projeto console application separado (FastFood.Auth.Migrator) responsável por executar migrations do Entity Framework Core. Este projeto será executado independentemente da API Lambda, permitindo aplicar migrations de forma isolada e segura, especialmente em ambientes de produção.

## Passos de implementação
- Criar novo projeto console application: `dotnet new console -n FastFood.Auth.Migrator -o src/FastFood.Auth.Migrator`
- Configurar o projeto `.csproj`:
  - Target Framework: `net8.0`
  - Adicionar referência ao projeto `FastFood.Auth.Infra.Persistence`
  - Adicionar pacote `Microsoft.EntityFrameworkCore.Design` (versão 8.0.0)
  - Adicionar pacotes `Microsoft.Extensions.Configuration`, `Microsoft.Extensions.Configuration.Json`, `Microsoft.Extensions.Configuration.EnvironmentVariables`
- Criar arquivo `Program.cs` que:
  - Carrega configuração de `appsettings.json` e `appsettings.Development.json`
  - Obtém connection string da configuração
  - Cria instância do `AuthDbContext` com connection string
  - Verifica migrations pendentes
  - Aplica migrations usando `context.Database.MigrateAsync()`
  - Exibe informações sobre migrations aplicadas
  - Trata erros adequadamente
- Criar arquivo `appsettings.json` com connection string (mesma estrutura do Lambda)
- Criar arquivo `appsettings.Development.json` com connection string para desenvolvimento
- Adicionar projeto à solução: `dotnet sln add src/FastFood.Auth.Migrator/FastFood.Auth.Migrator.csproj`
- Testar compilação: `dotnet build src/FastFood.Auth.Migrator`

## Como testar
- Executar `dotnet build` no projeto Migrator (deve compilar sem erros)
- Executar `dotnet run --project src/FastFood.Auth.Migrator` para aplicar migrations
- Verificar que o console exibe informações sobre migrations pendentes/aplicadas
- Conectar ao banco de dados e verificar que as migrations foram aplicadas
- Validar que o projeto pode ser executado independentemente da API Lambda

## Critérios de aceite
- Projeto `FastFood.Auth.Migrator` criado em `src/FastFood.Auth.Migrator/`
- Projeto é um console application (.NET 8)
- Projeto referencia `FastFood.Auth.Infra.Persistence`
- Pacotes necessários adicionados (EF Core Design, Configuration)
- `Program.cs` implementa lógica para aplicar migrations
- `Program.cs` carrega configuração de appsettings.json
- `Program.cs` exibe informações úteis sobre migrations
- `Program.cs` trata erros adequadamente
- Arquivos `appsettings.json` e `appsettings.Development.json` criados
- Projeto adicionado à solução
- Código compila sem erros
- Migrations podem ser aplicadas executando o projeto Migrator




