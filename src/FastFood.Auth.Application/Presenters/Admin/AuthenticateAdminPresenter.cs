using FastFood.Auth.Application.Responses.Admin;

namespace FastFood.Auth.Application.Presenters.Admin;

/// <summary>
/// Presenter para adaptar a resposta do UseCase AuthenticateAdminUseCase.
/// Por enquanto apenas retorna o response, mas preparado para transformações futuras se necessário.
/// </summary>
public class AuthenticateAdminPresenter
{
    /// <summary>
    /// Adapta a resposta do UseCase.
    /// </summary>
    /// <param name="response">Resposta do UseCase da camada Application</param>
    /// <returns>Response adaptado</returns>
    public AuthenticateAdminResponse Present(AuthenticateAdminResponse response)
    {
        // Por enquanto apenas retorna o response, mas preparado para transformações futuras
        return response;
    }
}



