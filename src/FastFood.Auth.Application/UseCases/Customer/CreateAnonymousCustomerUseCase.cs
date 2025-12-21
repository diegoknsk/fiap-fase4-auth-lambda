using FastFood.Auth.Application.OutputModels.Customer;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Application.Presenters.Customer;
using FastFood.Auth.Domain.Entities.CustomerIdentification;
using DomainCustomer = FastFood.Auth.Domain.Entities.CustomerIdentification.Customer;

namespace FastFood.Auth.Application.UseCases.Customer;

/// <summary>
/// UseCase para criar um customer anônimo e retornar um token JWT válido.
/// </summary>
public class CreateAnonymousCustomerUseCase(
    ICustomerRepository customerRepository,
    ITokenService tokenService)
{
    /// <summary>
    /// Executa a criação de um customer anônimo e gera um token JWT.
    /// </summary>
    public async Task<CreateAnonymousCustomerOutputModel> ExecuteAsync()
    {
        // Criar customer anônimo
        var customer = new DomainCustomer(
            id: Guid.NewGuid(),
            name: null,
            email: null,
            cpf: null,
            customerType: CustomerTypeEnum.Anonymous
        );

        // Salvar no repositório
        var savedCustomer = await customerRepository.AddAsync(customer);

        // Gerar token JWT
        var token = tokenService.GenerateToken(savedCustomer.Id, out var expiresAt);

        // Criar OutputModel
        var outputModel = new CreateAnonymousCustomerOutputModel
        {
            Token = token,
            CustomerId = savedCustomer.Id,
            ExpiresAt = expiresAt
        };

        // Chamar Presenter para transformar OutputModel em ResponseModel
        return CreateAnonymousCustomerPresenter.Present(outputModel);
    }
}

