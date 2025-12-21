using Microsoft.EntityFrameworkCore;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Domain.Entities.CustomerIdentification;
using FastFood.Auth.Domain.Entities.CustomerIdentification.ValueObects;
using FastFood.Auth.Infra.Persistence;
using FastFood.Auth.Infra.Persistence.Entities;

namespace FastFood.Auth.Infra.Persistence.Repositories;

/// <summary>
/// Implementação do repositório de Customer usando Entity Framework Core.
/// Responsável por fazer o mapeamento entre Customer (Domain) e CustomerEntity (Infra).
/// </summary>
public class CustomerRepository(AuthDbContext context) : ICustomerRepository
{

    public async Task<Customer?> GetByIdAsync(Guid id)
    {
        var entity = await context.Customers.FindAsync(id);
        return entity == null ? null : MapToDomain(entity);
    }

    public async Task<Customer?> GetByCpfAsync(string cpf)
    {
        var entity = await context.Customers
            .FirstOrDefaultAsync(c => c.Cpf == cpf);
        return entity == null ? null : MapToDomain(entity);
    }

    public async Task<bool> ExistsByCpfAsync(string cpf)
    {
        return await context.Customers
            .AnyAsync(c => c.Cpf == cpf);
    }

    public async Task<Customer> AddAsync(Customer customer)
    {
        var entity = MapToEntity(customer);
        await context.Customers.AddAsync(entity);
        await context.SaveChangesAsync();
        return MapToDomain(entity);
    }

    /// <summary>
    /// Mapeia CustomerEntity (Infra) para Customer (Domain)
    /// </summary>
    private static Customer MapToDomain(CustomerEntity entity)
    {
        Email? email = null;
        if (!string.IsNullOrWhiteSpace(entity.Email))
        {
            email = new Email(entity.Email);
        }

        Cpf? cpf = null;
        if (!string.IsNullOrWhiteSpace(entity.Cpf))
        {
            cpf = new Cpf(entity.Cpf);
        }

        var customer = new Customer(
            entity.Id,
            entity.Name,
            email,
            cpf,
            (CustomerTypeEnum)entity.CustomerType
        );

        // Preservar CreatedAt original do banco usando reflection
        var createdAtProperty = typeof(Customer).GetProperty(nameof(Customer.CreatedAt));
        if (createdAtProperty != null && createdAtProperty.SetMethod != null)
        {
            createdAtProperty.SetValue(customer, entity.CreatedAt);
        }

        return customer;
    }

    /// <summary>
    /// Mapeia Customer (Domain) para CustomerEntity (Infra)
    /// </summary>
    private static CustomerEntity MapToEntity(Customer customer)
    {
        return new CustomerEntity
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email?.Value,
            Cpf = customer.Cpf?.Value,
            CustomerType = (int)customer.CustomerType,
            CreatedAt = customer.CreatedAt
        };
    }
}

