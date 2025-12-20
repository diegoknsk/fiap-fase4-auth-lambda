namespace FastFood.Auth.Application.Ports;

/// <summary>
/// Interface para serviço de autenticação via AWS Cognito.
/// </summary>
public interface ICognitoService
{
    /// <summary>
    /// Autentica um usuário administrador através do AWS Cognito usando username e password.
    /// </summary>
    /// <param name="username">Username do administrador</param>
    /// <param name="password">Senha do administrador</param>
    /// <returns>Resultado da autenticação contendo os tokens</returns>
    Task<AuthenticateAdminResult> AuthenticateAsync(string username, string password);
}

/// <summary>
/// Resultado da autenticação de administrador via Cognito.
/// </summary>
public class AuthenticateAdminResult
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










