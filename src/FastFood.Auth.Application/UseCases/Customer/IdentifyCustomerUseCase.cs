using FastFood.Auth.Application.Commands.Customer;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Application.Responses.Customer;
using FastFood.Auth.Domain.Entities.CustomerIdentification.ValueObects;

namespace FastFood.Auth.Application.UseCases.Customer;

/// <summary>
/// UseCase para identificar um customer existente através do CPF e retornar um token JWT válido.
/// Se o customer não for encontrado, lança exceção para retornar 401 (Unauthorized).
/// </summary>
public class IdentifyCustomerUseCase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ITokenService _tokenService;

    public IdentifyCustomerUseCase(
        ICustomerRepository customerRepository,
        ITokenService tokenService)
    {
        _customerRepository = customerRepository;
        _tokenService = tokenService;
    }

    /// <summary>
    /// Executa a identificação de um customer através do CPF e gera um token JWT.
    /// </summary>
    public async Task<IdentifyCustomerResponse> ExecuteAsync(IdentifyCustomerCommand command)
    {
        // Validar CPF usando Value Object
        var cpfValueObject = new Cpf(command.Cpf);
        
        // Buscar customer existente pelo CPF
        var customer = await _customerRepository.GetByCpfAsync(cpfValueObject.Value!);

        // Se customer não encontrado, lançar exceção para retornar 401
        if (customer == null)
        {
            throw new UnauthorizedAccessException("Customer not found with the provided CPF.");
        }

        // Gerar token JWT
        var token = _tokenService.GenerateToken(customer.Id, out var expiresAt);

        // Retornar resposta
        return new IdentifyCustomerResponse
        {
            Token = token,
            CustomerId = customer.Id,
            ExpiresAt = expiresAt
        };
    }
}




