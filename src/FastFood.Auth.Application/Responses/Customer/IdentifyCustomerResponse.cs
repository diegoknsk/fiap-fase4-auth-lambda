namespace FastFood.Auth.Application.Responses.Customer;

/// <summary>
/// Resposta do UseCase IdentifyCustomerUseCase contendo o token JWT e informações do customer identificado.
/// </summary>
public class IdentifyCustomerResponse
{
    /// <summary>
    /// Token JWT gerado para autenticação
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Id do customer identificado
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Data de expiração do token
    /// </summary>
    public DateTime ExpiresAt { get; set; }
}




