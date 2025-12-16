namespace FastFood.Auth.Lambda.Models.Customer;

/// <summary>
/// Request model para registro de customer através do CPF.
/// </summary>
public class RegisterCustomerRequest
{
    /// <summary>
    /// CPF do customer a ser registrado
    /// </summary>
    public string Cpf { get; set; } = string.Empty;
}




