namespace FastFood.Auth.Application.OutputModels.Customer;

/// <summary>
/// OutputModel do UseCase RegisterCustomerUseCase contendo o token JWT e informações do customer registrado.
/// </summary>
public class RegisterCustomerOutputModel
{
    /// <summary>
    /// Token JWT gerado para autenticação
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Id do customer registrado
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Data de expiração do token
    /// </summary>
    public DateTime ExpiresAt { get; set; }
}









