using FastFood.Auth.Application.InputModels.Customer;
using FastFood.Auth.Application.OutputModels.Customer;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Application.Presenters.Customer;
using FastFood.Auth.Domain.Entities.CustomerIdentification;
using FastFood.Auth.Domain.Entities.CustomerIdentification.ValueObects;
using DomainCustomer = FastFood.Auth.Domain.Entities.CustomerIdentification.Customer;

namespace FastFood.Auth.Application.UseCases.Customer;

/// <summary>
/// UseCase para registrar um customer através do CPF e retornar um token JWT válido.
/// Se o customer já existir, retorna o token do customer existente.
/// Se não existir, cria um novo customer Registered e retorna um token.
/// </summary>
public class RegisterCustomerUseCase(
    ICustomerRepository customerRepository,
    ITokenService tokenService)
{
    /// <summary>
    /// Executa o registro de um customer através do CPF e gera um token JWT.
    /// </summary>
    public async Task<RegisterCustomerOutputModel> ExecuteAsync(RegisterCustomerInputModel inputModel)
    {
        // Validar CPF usando Value Object
        var cpfValueObject = new Cpf(inputModel.Cpf);
        
        // Buscar customer existente pelo CPF
        var existingCustomer = await customerRepository.GetByCpfAsync(cpfValueObject.Value!);

        DomainCustomer customer;
        
        if (existingCustomer != null)
        {
            // Se customer existe, usar o existente
            customer = existingCustomer;
        }
        else
        {
            // Se não existe, criar novo customer Registered
            customer = new DomainCustomer(
                id: Guid.NewGuid(),
                name: null,
                email: null,
                cpf: cpfValueObject,
                customerType: CustomerTypeEnum.Registered
            );

            // Salvar no repositório
            customer = await customerRepository.AddAsync(customer);
        }

        // Gerar token JWT
        var token = tokenService.GenerateToken(customer.Id, out var expiresAt);

        // Criar OutputModel
        var outputModel = new RegisterCustomerOutputModel
        {
            Token = token,
            CustomerId = customer.Id,
            ExpiresAt = expiresAt
        };

        // Chamar Presenter para transformar OutputModel em ResponseModel
        return RegisterCustomerPresenter.Present(outputModel);
    }
}




