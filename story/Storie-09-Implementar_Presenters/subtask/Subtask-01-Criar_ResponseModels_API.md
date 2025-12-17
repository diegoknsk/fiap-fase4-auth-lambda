# Subtask 01: Criar ResponseModels da API

## Descrição
Criar os ResponseModels na camada Lambda (API) que serão retornados pelos endpoints. Estes modelos são diferentes dos Responses da Application e representam o contrato da API.

## Passos de implementação
- Criar pasta `src/FastFood.Auth.Lambda/Models/Customer/` (se não existir)
- Criar arquivo `src/FastFood.Auth.Lambda/Models/Customer/RegisterCustomerResponse.cs`
- Criar arquivo `src/FastFood.Auth.Lambda/Models/Customer/IdentifyCustomerResponse.cs`
- Criar arquivo `src/FastFood.Auth.Lambda/Models/Customer/CreateAnonymousCustomerResponse.cs`
- Criar pasta `src/FastFood.Auth.Lambda/Models/Admin/` (se não existir)
- Criar arquivo `src/FastFood.Auth.Lambda/Models/Admin/AuthenticateAdminResponse.cs`

## Estrutura dos ResponseModels

### RegisterCustomerResponse
```csharp
public class RegisterCustomerResponse
{
    public string Token { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public DateTime ExpiresAt { get; set; }
}
```

### IdentifyCustomerResponse
```csharp
public class IdentifyCustomerResponse
{
    public string Token { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public DateTime ExpiresAt { get; set; }
}
```

### CreateAnonymousCustomerResponse
```csharp
public class CreateAnonymousCustomerResponse
{
    public string Token { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public DateTime ExpiresAt { get; set; }
}
```

### AuthenticateAdminResponse
```csharp
public class AuthenticateAdminResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string IdToken { get; set; } = string.Empty;
    public int ExpiresIn { get; set; }
    public string TokenType { get; set; } = "Bearer";
}
```

## Como testar
- Executar `dotnet build` no projeto Lambda (deve compilar sem erros)
- Verificar que os ResponseModels estão criados

## Critérios de aceite
- ResponseModels criados na camada Lambda
- Estrutura similar aos Application Responses mas em namespace diferente
- Código compila sem erros





