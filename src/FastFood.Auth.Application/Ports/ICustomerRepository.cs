using FastFood.Auth.Domain.Entities.CustomerIdentification;

namespace FastFood.Auth.Application.Ports;

/// <summary>
/// Port (interface) para repositório de Customer na camada Application.
/// Define os contratos para persistência de Customer seguindo o padrão de ports da Clean Architecture.
/// </summary>
public interface ICustomerRepository
{
    /// <summary>
    /// Busca um customer pelo Id
    /// </summary>
    Task<Customer?> GetByIdAsync(Guid id);

    /// <summary>
    /// Busca um customer pelo CPF
    /// </summary>
    Task<Customer?> GetByCpfAsync(string cpf);

    /// <summary>
    /// Verifica se existe um customer com o CPF informado
    /// </summary>
    Task<bool> ExistsByCpfAsync(string cpf);

    /// <summary>
    /// Adiciona um novo customer ao repositório
    /// </summary>
    Task<Customer> AddAsync(Customer customer);
}










