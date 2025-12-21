using FastFood.Auth.Application.InputModels.Customer;
using FastFood.Auth.Application.OutputModels.Customer;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Application.Presenters.Customer;
using FastFood.Auth.Domain.Entities.CustomerIdentification.ValueObects;

namespace FastFood.Auth.Application.UseCases.Customer;

/// <summary>
/// UseCase para identificar um customer existente através do CPF e retornar um token JWT válido.
/// Se o customer não for encontrado, lança exceção para retornar 401 (Unauthorized).
/// </summary>
public class IdentifyCustomerUseCase(
    ICustomerRepository customerRepository,
    ITokenService tokenService)
{
    /// <summary>
    /// Executa a identificação de um customer através do CPF e gera um token JWT.
    /// </summary>
    public async Task<IdentifyCustomerOutputModel> ExecuteAsync(IdentifyCustomerInputModel inputModel)
    {
        // Validar CPF usando Value Object
        var cpfValueObject = new Cpf(inputModel.Cpf);
        
        // Buscar customer existente pelo CPF
        var customer = await customerRepository.GetByCpfAsync(cpfValueObject.Value!);

        // Se customer não encontrado, lançar exceção para retornar 401
        if (customer == null)
        {
            throw new UnauthorizedAccessException("Customer not found with the provided CPF.");
        }

        // Gerar token JWT
        var token = tokenService.GenerateToken(customer.Id, out var expiresAt);

        // Criar OutputModel
        var outputModel = new IdentifyCustomerOutputModel
        {
            Token = token,
            CustomerId = customer.Id,
            ExpiresAt = expiresAt
        };

        // Chamar Presenter para transformar OutputModel em ResponseModel
        return IdentifyCustomerPresenter.Present(outputModel);
    }
}




