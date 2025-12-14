# Subtask 07: Registrar serviços e criar testes unitários

## Descrição
Registrar ICognitoService e AuthenticateAdminUseCase no Program.cs e criar testes unitários.

## Passos de implementação
- Abrir arquivo `src/FastFood.Auth.Lambda/Program.cs`
- Adicionar `AddScoped<ICognitoService, CognitoService>`
- Adicionar `AddScoped<AuthenticateAdminUseCase>`
- Criar arquivo de teste `tests/FastFood.Auth.Tests.Unit/UseCases/Admin/AuthenticateAdminUseCaseTests.cs`
- Criar testes usando Moq para mockar ICognitoService
- Testar cenários: autenticação bem-sucedida e credenciais inválidas

## Como testar
- Executar `dotnet build` (deve compilar sem erros)
- Executar `dotnet test` (deve passar)

## Critérios de aceite
- Serviços registrados no Program.cs
- Testes unitários criados
- `dotnet test` passa com sucesso

