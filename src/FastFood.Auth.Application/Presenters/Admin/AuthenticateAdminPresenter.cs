using FastFood.Auth.Application.OutputModels.Admin;

namespace FastFood.Auth.Application.Presenters.Admin;

/// <summary>
/// Presenter para adaptar a resposta do UseCase AuthenticateAdminUseCase.
/// Por enquanto apenas retorna o outputModel, mas preparado para transformações futuras se necessário.
/// </summary>
public class AuthenticateAdminPresenter
{
    /// <summary>
    /// Adapta a resposta do UseCase.
    /// </summary>
    /// <param name="outputModel">OutputModel do UseCase da camada Application</param>
    /// <returns>OutputModel adaptado</returns>
    public AuthenticateAdminOutputModel Present(AuthenticateAdminOutputModel outputModel)
    {
        // Por enquanto apenas retorna o outputModel, mas preparado para transformações futuras
        return outputModel;
    }
}



