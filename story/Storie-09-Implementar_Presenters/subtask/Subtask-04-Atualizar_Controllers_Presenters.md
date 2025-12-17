# Subtask 04: Atualizar Controllers para usar Presenters

## Descrição
Atualizar os controllers para usar os presenters ao invés de retornar diretamente os Application Responses.

## Passos de implementação
- Atualizar `src/FastFood.Auth.Lambda/Controllers/CustomerController.cs`:
  - Injetar presenters via construtor
  - Usar presenters nos métodos dos endpoints
  - Atualizar ProducesResponseType para usar os ResponseModels da API
- Atualizar `src/FastFood.Auth.Lambda/Controllers/AdminController.cs`:
  - Injetar presenter via construtor
  - Usar presenter no método do endpoint
  - Atualizar ProducesResponseType para usar o ResponseModel da API

## Exemplo de atualização

### CustomerController - Register
```csharp
private readonly RegisterCustomerPresenter _registerPresenter;

public CustomerController(
    CreateAnonymousCustomerUseCase createAnonymousUseCase,
    RegisterCustomerUseCase registerUseCase,
    IdentifyCustomerUseCase identifyUseCase,
    CreateAnonymousCustomerPresenter createAnonymousPresenter,
    RegisterCustomerPresenter registerPresenter,
    IdentifyCustomerPresenter identifyPresenter)
{
    // ... outros campos
    _registerPresenter = registerPresenter;
}

[HttpPost("register")]
[ProducesResponseType(typeof(Models.Customer.RegisterCustomerResponse), StatusCodes.Status200OK)]
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
- Controllers atualizados para usar presenters
- ProducesResponseType atualizado para usar ResponseModels da API
- Endpoints continuam funcionando corretamente
- Swagger documenta corretamente os ResponseModels
- Código compila sem erros




