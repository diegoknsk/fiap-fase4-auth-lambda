using FastFood.Auth.Application.InputModels.Admin;
using FastFood.Auth.Application.OutputModels.Admin;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Application.Presenters.Admin;

namespace FastFood.Auth.Application.UseCases.Admin;

/// <summary>
/// UseCase para autenticar um administrador via AWS Cognito.
/// </summary>
public class AuthenticateAdminUseCase(
    ICognitoService cognitoService)
{
    /// <summary>
    /// Executa a autenticação do administrador através do Cognito.
    /// </summary>
    /// <param name="inputModel">InputModel contendo username e password</param>
    /// <returns>OutputModel com os tokens de autenticação</returns>
    public async Task<AuthenticateAdminOutputModel> ExecuteAsync(AuthenticateAdminInputModel inputModel)
    {
        var result = await cognitoService.AuthenticateAsync(inputModel.Username, inputModel.Password);

        // Criar OutputModel
        var outputModel = new AuthenticateAdminOutputModel
        {
            AccessToken = result.AccessToken,
            IdToken = result.IdToken,
            ExpiresIn = result.ExpiresIn,
            TokenType = result.TokenType
        };

        // Chamar Presenter para transformar OutputModel em ResponseModel
        return AuthenticateAdminPresenter.Present(outputModel);
    }
}




