namespace FastFood.Auth.Application.OutputModels.Admin;

/// <summary>
/// OutputModel do UseCase AuthenticateAdminUseCase contendo os tokens de autenticação.
/// </summary>
public class AuthenticateAdminOutputModel
{
    /// <summary>
    /// Access Token retornado pelo Cognito
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// ID Token retornado pelo Cognito
    /// </summary>
    public string IdToken { get; set; } = string.Empty;

    /// <summary>
    /// Tempo de expiração do token em segundos
    /// </summary>
    public int ExpiresIn { get; set; }

    /// <summary>
    /// Tipo do token (geralmente "Bearer")
    /// </summary>
    public string TokenType { get; set; } = "Bearer";
}



