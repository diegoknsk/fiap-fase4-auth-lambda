namespace FastFood.Auth.Application.Commands.Admin;

/// <summary>
/// Command para autenticar um administrador via AWS Cognito.
/// </summary>
public class AuthenticateAdminCommand
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

