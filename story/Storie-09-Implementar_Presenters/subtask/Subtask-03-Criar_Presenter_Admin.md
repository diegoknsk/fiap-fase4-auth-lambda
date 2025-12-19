# Subtask 03: Criar Presenter para Admin

## Descrição
Criar presenter que faz a adaptação entre a resposta do UseCase de Admin (Application layer) e o ResponseModel da API (Lambda layer).

## Passos de implementação
- Criar pasta `src/FastFood.Auth.Lambda/Presenters/Admin/`
- Criar arquivo `src/FastFood.Auth.Lambda/Presenters/Admin/AuthenticateAdminPresenter.cs`

## Estrutura do Presenter

### AuthenticateAdminPresenter
```csharp
public class AuthenticateAdminPresenter
{
    public Models.Admin.AuthenticateAdminResponse Present(
        Application.Responses.Admin.AuthenticateAdminResponse response)
    {
        return new Models.Admin.AuthenticateAdminResponse
        {
            AccessToken = response.AccessToken,
            IdToken = response.IdToken,
            ExpiresIn = response.ExpiresIn,
            TokenType = response.TokenType
        };
    }
}
```

## Como testar
- Executar `dotnet build` no projeto Lambda (deve compilar sem erros)
- Verificar que o presenter está criado

## Critérios de aceite
- Presenter criado na camada Lambda
- Método Present implementado
- Código compila sem erros








