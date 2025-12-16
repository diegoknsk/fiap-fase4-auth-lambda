using FastFood.Auth.Application.Responses.Customer;

namespace FastFood.Auth.Application.Presenters.Customer;

/// <summary>
/// Presenter para adaptar a resposta do UseCase IdentifyCustomerUseCase.
/// Por enquanto apenas retorna o response, mas preparado para transformações futuras se necessário.
/// </summary>
public class IdentifyCustomerPresenter
{
    /// <summary>
    /// Adapta a resposta do UseCase.
    /// </summary>
    /// <param name="response">Resposta do UseCase da camada Application</param>
    /// <returns>Response adaptado</returns>
    public IdentifyCustomerResponse Present(IdentifyCustomerResponse response)
    {
        // Por enquanto apenas retorna o response, mas preparado para transformações futuras
        return response;
    }
}



