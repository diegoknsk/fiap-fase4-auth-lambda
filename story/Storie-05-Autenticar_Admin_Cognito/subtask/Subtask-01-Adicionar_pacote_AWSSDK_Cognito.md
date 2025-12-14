# Subtask 01: Adicionar pacote AWSSDK.CognitoIdentityProvider

## Descrição
Adicionar o pacote NuGet AWSSDK.CognitoIdentityProvider ao projeto que irá implementar o CognitoService (Infra ou Infra.Persistence).

## Passos de implementação
- Decidir onde implementar CognitoService (criar projeto Infra ou usar Infra.Persistence)
- Adicionar pacote `AWSSDK.CognitoIdentityProvider` ao projeto escolhido
- Verificar compatibilidade com .NET 8

## Como testar
- Executar `dotnet restore` (deve baixar pacote sem erros)
- Executar `dotnet build` (deve compilar sem erros)

## Critérios de aceite
- Pacote AWSSDK.CognitoIdentityProvider adicionado
- `dotnet restore` executa sem erros
- `dotnet build` compila sem erros

