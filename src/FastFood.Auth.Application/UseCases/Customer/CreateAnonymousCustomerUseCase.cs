using FastFood.Auth.Application.OutputModels.Customer;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Domain.Entities.CustomerIdentification;
using DomainCustomer = FastFood.Auth.Domain.Entities.CustomerIdentification.Customer;

namespace FastFood.Auth.Application.UseCases.Customer;

/// <summary>
/// UseCase para criar um customer anônimo e retornar um token JWT válido.
/// </summary>
public class CreateAnonymousCustomerUseCase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ITokenService _tokenService;

    public CreateAnonymousCustomerUseCase(
        ICustomerRepository customerRepository,
        ITokenService tokenService)
    {
        _customerRepository = customerRepository;
        _tokenService = tokenService;
    }

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
        var savedCustomer = await _customerRepository.AddAsync(customer);

        // Gerar token JWT
        var token = _tokenService.GenerateToken(savedCustomer.Id, out var expiresAt);

        // Retornar resposta
        return new CreateAnonymousCustomerOutputModel
        {
            Token = token,
            CustomerId = savedCustomer.Id,
            ExpiresAt = expiresAt
        };
    }
}

