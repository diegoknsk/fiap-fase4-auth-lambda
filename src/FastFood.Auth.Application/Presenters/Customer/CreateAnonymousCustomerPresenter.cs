using FastFood.Auth.Application.OutputModels.Customer;

namespace FastFood.Auth.Application.Presenters.Customer;

/// <summary>
/// Presenter para adaptar a resposta do UseCase CreateAnonymousCustomerUseCase.
/// Por enquanto apenas retorna o outputModel, mas preparado para transformações futuras se necessário.
/// </summary>
public class CreateAnonymousCustomerPresenter
{
    /// <summary>
    /// Adapta a resposta do UseCase.
    /// </summary>
    /// <param name="outputModel">OutputModel do UseCase da camada Application</param>
    /// <returns>OutputModel adaptado</returns>
    public CreateAnonymousCustomerOutputModel Present(CreateAnonymousCustomerOutputModel outputModel)
    {
        // Por enquanto apenas retorna o outputModel, mas preparado para transformações futuras
        return outputModel;
    }
}



