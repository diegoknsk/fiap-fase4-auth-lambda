namespace FastFood.Auth.Application.Commands.Customer;

/// <summary>
/// Command para registrar um customer através do CPF.
/// </summary>
public class RegisterCustomerCommand
{
    /// <summary>
    /// CPF do customer a ser registrado
    /// </summary>
    public string Cpf { get; set; } = string.Empty;
}


