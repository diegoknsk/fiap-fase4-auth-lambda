namespace FastFood.Auth.Application.Commands.Customer;

/// <summary>
/// Command para identificar um customer através do CPF.
/// </summary>
public class IdentifyCustomerCommand
{
    /// <summary>
    /// CPF do customer a ser identificado
    /// </summary>
    public string Cpf { get; set; } = string.Empty;
}




