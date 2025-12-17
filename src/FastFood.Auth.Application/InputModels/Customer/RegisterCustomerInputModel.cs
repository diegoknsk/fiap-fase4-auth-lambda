namespace FastFood.Auth.Application.InputModels.Customer;

/// <summary>
/// InputModel para registrar um customer através do CPF.
/// </summary>
public class RegisterCustomerInputModel
{
    /// <summary>
    /// CPF do customer a ser registrado
    /// </summary>
    public string Cpf { get; set; } = string.Empty;
}


