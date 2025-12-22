namespace FastFood.Auth.Application.InputModels.Admin;

/// <summary>
/// InputModel para autenticar um administrador via AWS Cognito.
/// </summary>
public class AuthenticateAdminInputModel
{
    /// <summary>
    /// Username do administrador
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Senha do administrador
    /// </summary>
    public string Password { get; set; } = string.Empty;
}









