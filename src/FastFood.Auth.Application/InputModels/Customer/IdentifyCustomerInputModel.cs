namespace FastFood.Auth.Application.InputModels.Customer;

/// <summary>
/// InputModel para identificar um customer através do CPF.
/// </summary>
public class IdentifyCustomerInputModel
{
    /// <summary>
    /// CPF do customer a ser identificado
    /// </summary>
    public string Cpf { get; set; } = string.Empty;
}






