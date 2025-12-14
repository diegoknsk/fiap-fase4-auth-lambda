# Subtask 06: Criar Controller CustomerController com endpoint anonymous

## Descrição
Criar Controller CustomerController com endpoint POST `/customer/anonymous` que chama o UseCase CreateAnonymousCustomerUseCase e retorna a resposta com token.

## Passos de implementação
- Criar arquivo `src/FastFood.Auth.Lambda/Controllers/CustomerController.cs`
- Criar classe `CustomerController` herdando de `ControllerBase`
- Adicionar atributos `[ApiController]` e `[Route("api/[controller]")]`
- Adicionar campo privado `_useCase` do tipo `CreateAnonymousCustomerUseCase` (injetado via construtor)
- Criar método `[HttpPost("anonymous")]` que:
  - Chama `await _useCase.ExecuteAsync()`
  - Retorna `Ok(result)` com a resposta
- Adicionar tratamento de exceções (try-catch) retornando StatusCode 500 em caso de erro
- Adicionar documentação XML para Swagger (opcional mas recomendado)

## Como testar
- Executar `dotnet build` no projeto Lambda (deve compilar sem erros)
- Executar a aplicação e verificar que o endpoint aparece no Swagger
- Validar que o endpoint está acessível em `/api/customer/anonymous`

## Critérios de aceite
- Arquivo CustomerController.cs criado
- Classe herda de ControllerBase
- Atributos [ApiController] e [Route] aplicados
- Endpoint POST `/customer/anonymous` criado
- UseCase injetado via construtor
- Método chama UseCase e retorna resposta
- Tratamento de exceções implementado
- Endpoint aparece no Swagger
- Código compila sem erros

