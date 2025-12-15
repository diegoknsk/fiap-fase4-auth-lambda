# Subtask 01: Mover Presenters para Application

## Descrição
Mover os presenters da camada Lambda para a camada Application, seguindo o princípio de que a Application define o contrato de saída.

## Passos de implementação
- Criar pasta `src/FastFood.Auth.Application/Presenters/Customer/`
- Criar pasta `src/FastFood.Auth.Application/Presenters/Admin/`
- Mover `src/FastFood.Auth.Lambda/Presenters/Customer/RegisterCustomerPresenter.cs` para `src/FastFood.Auth.Application/Presenters/Customer/`
- Mover `src/FastFood.Auth.Lambda/Presenters/Customer/IdentifyCustomerPresenter.cs` para `src/FastFood.Auth.Application/Presenters/Customer/`
- Mover `src/FastFood.Auth.Lambda/Presenters/Customer/CreateAnonymousCustomerPresenter.cs` para `src/FastFood.Auth.Application/Presenters/Customer/`
- Mover `src/FastFood.Auth.Lambda/Presenters/Admin/AuthenticateAdminPresenter.cs` para `src/FastFood.Auth.Application/Presenters/Admin/`
- Atualizar namespaces dos presenters para `FastFood.Auth.Application.Presenters.*`
- Atualizar referências nos presenters para usar Application Responses diretamente (sem duplicação)

## Estrutura dos Presenters na Application

Os presenters devem retornar os Application Responses diretamente, preparando para futuras transformações se necessário:

```csharp
namespace FastFood.Auth.Application.Presenters.Customer;

public class RegisterCustomerPresenter
{
    public Responses.Customer.RegisterCustomerResponse Present(
        Responses.Customer.RegisterCustomerResponse response)
    {
        // Por enquanto apenas retorna o response, mas preparado para transformações futuras
        return response;
    }
}
```

## Como testar
- Executar `dotnet build` no projeto Application (deve compilar sem erros)
- Verificar que os presenters estão na camada Application

## Critérios de aceite
- Presenters movidos para Application
- Namespaces atualizados
- Código compila sem erros

