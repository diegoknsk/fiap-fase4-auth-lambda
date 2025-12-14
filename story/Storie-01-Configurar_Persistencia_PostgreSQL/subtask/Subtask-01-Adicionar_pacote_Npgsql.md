# Subtask 01: Adicionar pacote Npgsql.EntityFrameworkCore.PostgreSQL

## Descrição
Adicionar o pacote NuGet Npgsql.EntityFrameworkCore.PostgreSQL ao projeto FastFood.Auth.Infra.Persistence para habilitar suporte ao PostgreSQL no Entity Framework Core.

## Passos de implementação
- Abrir o arquivo `src/FastFood.Auth.Infra.Persistence/FastFood.Auth.Infra.Persistence.csproj`
- Adicionar referência ao pacote `Npgsql.EntityFrameworkCore.PostgreSQL` na versão mais recente compatível com .NET 8
- Adicionar referência ao pacote `Microsoft.EntityFrameworkCore.Design` (necessário para migrations)
- Verificar que o projeto referencia corretamente `FastFood.Auth.Domain`

## Como testar
- Executar `dotnet restore` na solução (deve baixar os pacotes sem erros)
- Executar `dotnet build` no projeto Infra.Persistence (deve compilar sem erros)
- Verificar que os pacotes aparecem em `obj/project.assets.json`

## Critérios de aceite
- Pacote Npgsql.EntityFrameworkCore.PostgreSQL adicionado ao .csproj
- Pacote Microsoft.EntityFrameworkCore.Design adicionado ao .csproj
- `dotnet restore` executa sem erros
- `dotnet build` compila o projeto sem erros

