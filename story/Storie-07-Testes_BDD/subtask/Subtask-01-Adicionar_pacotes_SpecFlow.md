# Subtask 01: Adicionar pacotes SpecFlow ao projeto BDD

## Descrição
Adicionar pacotes NuGet necessários para testes BDD usando SpecFlow com xUnit no projeto FastFood.Auth.Tests.Bdd.

## Passos de implementação
- Abrir arquivo `tests/FastFood.Auth.Tests.Bdd/FastFood.Auth.Tests.Bdd.csproj`
- Adicionar pacotes:
  - `SpecFlow` (versão mais recente compatível com .NET 8)
  - `SpecFlow.xUnit` (integração com xUnit)
  - `SpecFlow.Tools.MsBuild.Generation` (geração de código)
- Adicionar referências de projeto:
  - `FastFood.Auth.Domain`
  - `FastFood.Auth.Application`
  - `FastFood.Auth.Lambda`
  - `FastFood.Auth.Infra.Persistence` (se necessário para testes de integração)
- Adicionar pacote `Microsoft.AspNetCore.Mvc.Testing` (para testar controllers em memória)
- Executar `dotnet restore` para baixar os pacotes

## Como testar
- Executar `dotnet restore` na solução (deve baixar os pacotes sem erros)
- Executar `dotnet build` no projeto BDD (deve compilar sem erros)
- Verificar que os pacotes aparecem em `obj/project.assets.json`

## Critérios de aceite
- Pacotes SpecFlow, SpecFlow.xUnit e SpecFlow.Tools.MsBuild.Generation adicionados
- Pacote Microsoft.AspNetCore.Mvc.Testing adicionado
- Referências de projeto adicionadas
- `dotnet restore` executa sem erros
- `dotnet build` compila o projeto sem erros

