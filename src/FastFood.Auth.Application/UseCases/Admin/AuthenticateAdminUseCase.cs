using FastFood.Auth.Application.Commands.Admin;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Application.Responses.Admin;

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
    /// <param name="command">Command contendo username e password</param>
    /// <returns>Resposta com os tokens de autenticação</returns>
    public async Task<AuthenticateAdminResponse> ExecuteAsync(AuthenticateAdminCommand command)
    {
        var result = await _cognitoService.AuthenticateAsync(command.Username, command.Password);

        return new AuthenticateAdminResponse
        {
            AccessToken = result.AccessToken,
            IdToken = result.IdToken,
            ExpiresIn = result.ExpiresIn,
            TokenType = result.TokenType
        };
    }
}

