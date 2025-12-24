using FastFood.Auth.Application.OutputModels.Admin;

namespace FastFood.Auth.Application.Presenters.Admin;

/// <summary>
/// Presenter para adaptar a resposta do UseCase RunMigrationsUseCase.
/// Por enquanto apenas retorna o outputModel, mas preparado para transformações futuras se necessário.
/// </summary>
public static class RunMigrationsPresenter
{
    /// <summary>
    /// Adapta a resposta do UseCase.
    /// </summary>
    /// <param name="outputModel">OutputModel do UseCase da camada Application</param>
    /// <returns>OutputModel adaptado</returns>
    public static RunMigrationsOutputModel Present(RunMigrationsOutputModel outputModel)
    {
        // Por enquanto apenas retorna o outputModel, mas preparado para transformações futuras
        return outputModel;
    }
}


