# Subtask 02: Criar Presenters para Customer

## Descrição
Criar presenters que fazem a adaptação entre as respostas dos UseCases de Customer (Application layer) e os ResponseModels da API (Lambda layer).

## Passos de implementação
- Criar pasta `src/FastFood.Auth.Lambda/Presenters/Customer/`
- Criar arquivo `src/FastFood.Auth.Lambda/Presenters/Customer/RegisterCustomerPresenter.cs`
- Criar arquivo `src/FastFood.Auth.Lambda/Presenters/Customer/IdentifyCustomerPresenter.cs`
- Criar arquivo `src/FastFood.Auth.Lambda/Presenters/Customer/CreateAnonymousCustomerPresenter.cs`

## Estrutura dos Presenters

Cada presenter deve ter um método `Present` que recebe o Application Response e retorna o API ResponseModel.

### RegisterCustomerPresenter
```csharp
public class RegisterCustomerPresenter
{
    public Models.Customer.RegisterCustomerResponse Present(
        Application.Responses.Customer.RegisterCustomerResponse response)
    {
        return new Models.Customer.RegisterCustomerResponse
        {
            Token = response.Token,
            CustomerId = response.CustomerId,
            ExpiresAt = response.ExpiresAt
        };
    }
}
```

### IdentifyCustomerPresenter
```csharp
public class IdentifyCustomerPresenter
{
    public Models.Customer.IdentifyCustomerResponse Present(
        Application.Responses.Customer.IdentifyCustomerResponse response)
    {
        return new Models.Customer.IdentifyCustomerResponse
        {
            Token = response.Token,
            CustomerId = response.CustomerId,
            ExpiresAt = response.ExpiresAt
        };
    }
}
```

### CreateAnonymousCustomerPresenter
```csharp
public class CreateAnonymousCustomerPresenter
{
    public Models.Customer.CreateAnonymousCustomerResponse Present(
        Application.Responses.Customer.CreateAnonymousCustomerResponse response)
    {
        return new Models.Customer.CreateAnonymousCustomerResponse
        {
            Token = response.Token,
            CustomerId = response.CustomerId,
            ExpiresAt = response.ExpiresAt
        };
    }
}
```

## Como testar
- Executar `dotnet build` no projeto Lambda (deve compilar sem erros)
- Verificar que os presenters estão criados

## Critérios de aceite
- Presenters criados na camada Lambda
- Método Present implementado para cada presenter
- Código compila sem erros












