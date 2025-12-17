using FastFood.Auth.Application.OutputModels.Customer;

namespace FastFood.Auth.Application.Presenters.Customer;

/// <summary>
/// Presenter para adaptar a resposta do UseCase RegisterCustomerUseCase.
/// Por enquanto apenas retorna o outputModel, mas preparado para transformações futuras se necessário.
/// </summary>
public class RegisterCustomerPresenter
{
    /// <summary>
    /// Adapta a resposta do UseCase.
    /// </summary>
    /// <param name="outputModel">OutputModel do UseCase da camada Application</param>
    /// <returns>OutputModel adaptado</returns>
    public RegisterCustomerOutputModel Present(RegisterCustomerOutputModel outputModel)
    {
        // Por enquanto apenas retorna o outputModel, mas preparado para transformações futuras
        return outputModel;
    }
}



