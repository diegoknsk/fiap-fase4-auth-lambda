# Subtask 05: Criar Controller AdminController com endpoint login

## Descrição
Criar Controller AdminController com endpoint POST `/admin/login` que recebe username/password e chama o UseCase AuthenticateAdminUseCase.

## Passos de implementação
- Criar arquivo `src/FastFood.Auth.Lambda/Controllers/AdminController.cs`
- Criar classe `AdminController` herdando de `ControllerBase`
- Adicionar atributos `[ApiController]` e `[Route("api/[controller]")]`
- Adicionar campo privado `_useCase` do tipo `AuthenticateAdminUseCase` (injetado via construtor)
- Criar método `[HttpPost("login")]` que:
  - Recebe `AdminLoginRequest` do body
  - Mapeia para `AuthenticateAdminCommand`
  - Chama `await _useCase.ExecuteAsync(command)`
  - Retorna `Ok(result)` ou `Unauthorized` se exceção
- Criar classe `AdminLoginRequest` em `Models/Admin/` com Username e Password

## Como testar
- Executar `dotnet build` (deve compilar sem erros)
- Executar aplicação e verificar endpoint no Swagger

## Critérios de aceite
- Arquivo AdminController.cs criado
- Endpoint POST `/admin/login` criado
- Endpoint retorna tokens quando autenticação bem-sucedida
- Endpoint retorna 401 quando credenciais inválidas
- Endpoint aparece no Swagger
- Código compila sem erros

