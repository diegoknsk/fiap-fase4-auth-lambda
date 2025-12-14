# Subtask 06: Criar testes unitários para Controllers

## Descrição
Criar testes unitários para os Controllers (CustomerController e AdminController) validando contratos HTTP, mapeamentos e respostas.

## Passos de implementação
- Criar diretório `tests/FastFood.Auth.Tests.Unit/Controllers/` se não existir
- Criar arquivo `CustomerControllerTests.cs` com testes:
  - `PostAnonymous_ShouldReturnOkWithToken` - POST /anonymous retorna 200 com token
  - `PostAnonymous_ShouldCallUseCase` - Deve chamar CreateAnonymousCustomerUseCase
  - `PostRegister_WithValidCpf_ShouldReturnOkWithToken` - POST /register retorna 200
  - `PostRegister_ShouldCallUseCase` - Deve chamar RegisterCustomerUseCase
  - `PostIdentify_WithValidCpf_ShouldReturnOkWithToken` - POST /identify retorna 200
  - `PostIdentify_WithInvalidCpf_ShouldReturnUnauthorized` - POST /identify retorna 401
  - `PostIdentify_ShouldCallUseCase` - Deve chamar IdentifyCustomerUseCase
- Criar arquivo `AdminControllerTests.cs` com testes:
  - `PostLogin_WithValidCredentials_ShouldReturnOkWithTokens` - POST /login retorna 200 com tokens
  - `PostLogin_WithInvalidCredentials_ShouldReturnUnauthorized` - POST /login retorna 401
  - `PostLogin_ShouldCallUseCase` - Deve chamar AuthenticateAdminUseCase

## Como testar
- Executar `dotnet test --filter "FullyQualifiedName~Controllers"` (deve passar todos os testes)
- Verificar que controllers são testados isoladamente
- Validar que respostas HTTP estão corretas

## Critérios de aceite
- Arquivo CustomerControllerTests.cs criado com pelo menos 7 testes
- Arquivo AdminControllerTests.cs criado com pelo menos 3 testes
- Testes usam Moq para mockar UseCases
- Testes validam códigos HTTP (200, 401)
- Testes validam estrutura das respostas
- Todos os testes passando
- `dotnet test` passa com sucesso

