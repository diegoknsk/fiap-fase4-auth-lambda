namespace FastFood.Auth.Application.OutputModels.Customer;

/// <summary>
/// OutputModel do UseCase CreateAnonymousCustomerUseCase contendo o token JWT e informações do customer criado.
/// </summary>
public class CreateAnonymousCustomerOutputModel
{
    /// <summary>
    /// Token JWT gerado para autenticação
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Id do customer anônimo criado
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Data de expiração do token
    /// </summary>
    public DateTime ExpiresAt { get; set; }
}










