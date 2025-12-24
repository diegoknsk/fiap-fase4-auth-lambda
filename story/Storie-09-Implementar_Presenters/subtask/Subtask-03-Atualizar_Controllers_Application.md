# Subtask 03: Atualizar Controllers para usar Presenters da Application

## Descrição
Atualizar os controllers para usar os presenters da camada Application ao invés dos presenters da Lambda.

## Passos de implementação
- Atualizar `src/FastFood.Auth.Lambda/Controllers/CustomerController.cs`:
  - Atualizar imports para usar `FastFood.Auth.Application.Presenters.Customer`
  - Atualizar ProducesResponseType para usar `Application.Responses.Customer.*`
  - Manter a injeção de presenters via construtor
- Atualizar `src/FastFood.Auth.Lambda/Controllers/AdminController.cs`:
  - Atualizar imports para usar `FastFood.Auth.Application.Presenters.Admin`
  - Atualizar ProducesResponseType para usar `Application.Responses.Admin.*`
  - Manter a injeção de presenter via construtor
- Atualizar `src/FastFood.Auth.Lambda/Program.cs`:
  - Atualizar imports para usar `FastFood.Auth.Application.Presenters.*`
  - Atualizar registro de presenters para usar os da Application

## Exemplo de atualização

### CustomerController
```csharp
using FastFood.Auth.Application.Presenters.Customer;
using FastFood.Auth.Application.Responses.Customer;

[HttpPost("register")]
[ProducesResponseType(typeof(RegisterCustomerResponse), StatusCodes.Status200OK)]
public async Task<IActionResult> Register([FromBody] RegisterCustomerRequest request)
{
    var result = await _registerUseCase.ExecuteAsync(command);
    var response = _registerPresenter.Present(result);
    return Ok(response);
}
```

## Como testar
- Executar `dotnet build` no projeto Lambda (deve compilar sem erros)
- Executar a aplicação e testar os endpoints via Swagger
- Verificar que as respostas estão corretas

## Critérios de aceite
- Controllers atualizados para usar presenters da Application
- ProducesResponseType atualizado para usar Application Responses
- Program.cs atualizado com registros corretos
- Endpoints continuam funcionando corretamente
- Swagger documenta corretamente os Application Responses
- Código compila sem erros












