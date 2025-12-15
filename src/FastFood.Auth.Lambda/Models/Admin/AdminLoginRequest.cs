namespace FastFood.Auth.Lambda.Models.Admin;

/// <summary>
/// Request model para autenticação de administrador.
/// </summary>
public class AdminLoginRequest
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


