using FastFood.Auth.Application.OutputModels.Customer;

namespace FastFood.Auth.Application.Presenters.Customer;

/// <summary>
/// Presenter para adaptar a resposta do UseCase IdentifyCustomerUseCase.
/// Por enquanto apenas retorna o outputModel, mas preparado para transformações futuras se necessário.
/// </summary>
public static class IdentifyCustomerPresenter
{
    /// <summary>
    /// Adapta a resposta do UseCase.
    /// </summary>
    /// <param name="outputModel">OutputModel do UseCase da camada Application</param>
    /// <returns>OutputModel adaptado</returns>
    public static IdentifyCustomerOutputModel Present(IdentifyCustomerOutputModel outputModel)
    {
        // Por enquanto apenas retorna o outputModel, mas preparado para transformações futuras
        return outputModel;
    }
}



