# Subtask 01: Adicionar pacote Moq ao projeto de testes

## Descrição
Adicionar o pacote NuGet Moq ao projeto FastFood.Auth.Tests.Unit para permitir criação de mocks nas dependências dos testes.

## Passos de implementação
- Abrir arquivo `tests/FastFood.Auth.Tests.Unit/FastFood.Auth.Tests.Unit.csproj`
- Adicionar referência ao pacote `Moq` na versão mais recente compatível com .NET 8
- Adicionar referências de projeto aos projetos que serão testados:
  - `FastFood.Auth.Domain`
  - `FastFood.Auth.Application`
  - `FastFood.Auth.Lambda` (para testes de controllers)
- Executar `dotnet restore` para baixar os pacotes

## Como testar
- Executar `dotnet restore` na solução (deve baixar os pacotes sem erros)
- Executar `dotnet build` no projeto de testes (deve compilar sem erros)
- Verificar que os pacotes aparecem em `obj/project.assets.json`

## Critérios de aceite
- Pacote Moq adicionado ao .csproj
- Referências de projeto adicionadas (Domain, Application, Lambda)
- `dotnet restore` executa sem erros
- `dotnet build` compila o projeto sem erros

