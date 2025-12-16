using FastFood.Auth.Application.Responses.Customer;

namespace FastFood.Auth.Application.Presenters.Customer;

/// <summary>
/// Presenter para adaptar a resposta do UseCase RegisterCustomerUseCase.
/// Por enquanto apenas retorna o response, mas preparado para transformações futuras se necessário.
/// </summary>
public class RegisterCustomerPresenter
{
    /// <summary>
    /// Adapta a resposta do UseCase.
    /// </summary>
    /// <param name="response">Resposta do UseCase da camada Application</param>
    /// <returns>Response adaptado</returns>
    public RegisterCustomerResponse Present(RegisterCustomerResponse response)
    {
        // Por enquanto apenas retorna o response, mas preparado para transformações futuras
        return response;
    }
}



