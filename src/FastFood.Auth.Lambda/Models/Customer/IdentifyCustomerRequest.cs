namespace FastFood.Auth.Lambda.Models.Customer;

/// <summary>
/// Request model para identificação de customer através do CPF.
/// </summary>
public class IdentifyCustomerRequest
{
    /// <summary>
    /// CPF do customer a ser identificado
    /// </summary>
    public string Cpf { get; set; } = string.Empty;
}


