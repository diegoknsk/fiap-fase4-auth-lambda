using FastFood.Auth.Application.InputModels.Admin;
using FastFood.Auth.Application.OutputModels.Admin;
using FastFood.Auth.Application.Ports;

namespace FastFood.Auth.Application.UseCases.Admin;

/// <summary>
/// UseCase para autenticar um administrador via AWS Cognito.
/// </summary>
public class AuthenticateAdminUseCase
{
    private readonly ICognitoService _cognitoService;

    public AuthenticateAdminUseCase(ICognitoService cognitoService)
    {
        _cognitoService = cognitoService;
    }

    /// <summary>
    /// Executa a autenticação do administrador através do Cognito.
    /// </summary>
    /// <param name="inputModel">InputModel contendo username e password</param>
    /// <returns>OutputModel com os tokens de autenticação</returns>
    public async Task<AuthenticateAdminOutputModel> ExecuteAsync(AuthenticateAdminInputModel inputModel)
    {
        var result = await _cognitoService.AuthenticateAsync(inputModel.Username, inputModel.Password);

        return new AuthenticateAdminOutputModel
        {
            AccessToken = result.AccessToken,
            IdToken = result.IdToken,
            ExpiresIn = result.ExpiresIn,
            TokenType = result.TokenType
        };
    }
}




