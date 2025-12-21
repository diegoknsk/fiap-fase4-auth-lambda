using FastFood.Auth.Domain.Entities.CustomerIdentification;
using FastFood.Auth.Domain.Entities.CustomerIdentification.ValueObects;
using FastFood.Auth.Infra.Persistence;
using FastFood.Auth.Infra.Persistence.Entities;
using FastFood.Auth.Infra.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Auth.Tests.Unit.Infra.Persistence.Repositories;

/// <summary>
/// Testes unitários para CustomerRepository.
/// </summary>
public class CustomerRepositoryTests : IDisposable
{
    private readonly AuthDbContext _context;
    private readonly CustomerRepository _repository;

    public CustomerRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AuthDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AuthDbContext(options);
        _repository = new CustomerRepository(_context);
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingId_ShouldReturnCustomer()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var entity = new CustomerEntity
        {
            Id = customerId,
            Name = "Test Customer",
            Email = "test@example.com",
            Cpf = "11144477735",
            CustomerType = (int)CustomerType.Registered,
            CreatedAt = DateTime.UtcNow
        };

        _context.Customers.Add(entity);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(customerId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(customerId, result.Id);
        Assert.Equal("Test Customer", result.Name);
        Assert.NotNull(result.Email);
        Assert.Equal("test@example.com", result.Email.Value);
        Assert.NotNull(result.Cpf);
        Assert.Equal("11144477735", result.Cpf.Value);
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistingId_ShouldReturnNull()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        var result = await _repository.GetByIdAsync(nonExistingId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldMapEntityToDomainCorrectly()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var createdAt = DateTime.UtcNow.AddDays(-1);
        var entity = new CustomerEntity
        {
            Id = customerId,
            Name = "Mapped Customer",
            Email = "mapped@example.com",
            Cpf = "11144477735",
            CustomerType = (int)CustomerType.Registered,
            CreatedAt = createdAt
        };

        _context.Customers.Add(entity);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(customerId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(customerId, result.Id);
        Assert.Equal("Mapped Customer", result.Name);
        Assert.NotNull(result.Email);
        Assert.Equal("mapped@example.com", result.Email.Value);
        Assert.NotNull(result.Cpf);
        Assert.Equal("11144477735", result.Cpf.Value);
        Assert.Equal(CustomerType.Registered, result.CustomerType);
        Assert.Equal(createdAt, result.CreatedAt);
    }

    [Fact]
    public async Task GetByIdAsync_WithCustomerWithEmail_ShouldMapEmailCorrectly()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var entity = new CustomerEntity
        {
            Id = customerId,
            Name = "Email Customer",
            Email = "email@test.com",
            CustomerType = (int)CustomerType.Registered,
            CreatedAt = DateTime.UtcNow
        };

        _context.Customers.Add(entity);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(customerId);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Email);
        Assert.Equal("email@test.com", result.Email.Value);
    }

    [Fact]
    public async Task GetByIdAsync_WithCustomerWithCpf_ShouldMapCpfCorrectly()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var entity = new CustomerEntity
        {
            Id = customerId,
            Name = "CPF Customer",
            Cpf = "11144477735",
            CustomerType = (int)CustomerType.Registered,
            CreatedAt = DateTime.UtcNow
        };

        _context.Customers.Add(entity);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(customerId);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Cpf);
        Assert.Equal("11144477735", result.Cpf.Value);
    }

    [Fact]
    public async Task GetByIdAsync_WithCustomerWithoutEmailAndCpf_ShouldMapNullsCorrectly()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var entity = new CustomerEntity
        {
            Id = customerId,
            Name = null,
            Email = null,
            Cpf = null,
            CustomerType = (int)CustomerType.Anonymous,
            CreatedAt = DateTime.UtcNow
        };

        _context.Customers.Add(entity);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(customerId);

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.Email);
        Assert.Null(result.Cpf);
        Assert.Equal(CustomerType.Anonymous, result.CustomerType);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldPreserveCreatedAt()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var createdAt = DateTime.UtcNow.AddDays(-5);
        var entity = new CustomerEntity
        {
            Id = customerId,
            Name = "Created At Customer",
            CustomerType = (int)CustomerType.Registered,
            CreatedAt = createdAt
        };

        _context.Customers.Add(entity);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(customerId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createdAt, result.CreatedAt);
    }

    [Fact]
    public async Task GetByCpfAsync_WithExistingCpf_ShouldReturnCustomer()
    {
        // Arrange
        var cpf = "11144477735";
        var entity = new CustomerEntity
        {
            Id = Guid.NewGuid(),
            Name = "CPF Test Customer",
            Cpf = cpf,
            CustomerType = (int)CustomerType.Registered,
            CreatedAt = DateTime.UtcNow
        };

        _context.Customers.Add(entity);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByCpfAsync(cpf);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Cpf);
        Assert.Equal(cpf, result.Cpf.Value);
        Assert.Equal("CPF Test Customer", result.Name);
    }

    [Fact]
    public async Task GetByCpfAsync_WithNonExistingCpf_ShouldReturnNull()
    {
        // Arrange
        var nonExistingCpf = "99999999999";

        // Act
        var result = await _repository.GetByCpfAsync(nonExistingCpf);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByCpfAsync_ShouldMapEntityToDomainCorrectly()
    {
        // Arrange
        var cpf = "11144477735";
        var entity = new CustomerEntity
        {
            Id = Guid.NewGuid(),
            Name = "CPF Mapped Customer",
            Email = "cpf@example.com",
            Cpf = cpf,
            CustomerType = (int)CustomerType.Registered,
            CreatedAt = DateTime.UtcNow
        };

        _context.Customers.Add(entity);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByCpfAsync(cpf);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("CPF Mapped Customer", result.Name);
        Assert.NotNull(result.Email);
        Assert.Equal("cpf@example.com", result.Email.Value);
        Assert.NotNull(result.Cpf);
        Assert.Equal(cpf, result.Cpf.Value);
    }

    [Fact]
    public async Task GetByCpfAsync_WithFormattedCpf_ShouldFindCustomer()
    {
        // Arrange
        var cpf = "11144477735";
        var entity = new CustomerEntity
        {
            Id = Guid.NewGuid(),
            Name = "Formatted CPF Customer",
            Cpf = cpf,
            CustomerType = (int)CustomerType.Registered,
            CreatedAt = DateTime.UtcNow
        };

        _context.Customers.Add(entity);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByCpfAsync(cpf);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(cpf, result.Cpf!.Value);
    }

    [Fact]
    public async Task ExistsByCpfAsync_WithExistingCpf_ShouldReturnTrue()
    {
        // Arrange
        var cpf = "11144477735";
        var entity = new CustomerEntity
        {
            Id = Guid.NewGuid(),
            Cpf = cpf,
            CustomerType = (int)CustomerType.Registered,
            CreatedAt = DateTime.UtcNow
        };

        _context.Customers.Add(entity);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.ExistsByCpfAsync(cpf);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExistsByCpfAsync_WithNonExistingCpf_ShouldReturnFalse()
    {
        // Arrange
        var nonExistingCpf = "99999999999";

        // Act
        var result = await _repository.ExistsByCpfAsync(nonExistingCpf);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task AddAsync_WithNewCustomer_ShouldSaveAndReturnCustomer()
    {
        // Arrange
        var customer = new Customer(
            id: Guid.NewGuid(),
            name: "New Customer",
            email: new Email("new@example.com"),
            cpf: new Cpf("11144477735"),
            customerType: CustomerType.Registered
        );

        // Act
        var result = await _repository.AddAsync(customer);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(customer.Id, result.Id);
        Assert.Equal("New Customer", result.Name);
        Assert.NotNull(result.Email);
        Assert.Equal("new@example.com", result.Email.Value);
        Assert.NotNull(result.Cpf);
        Assert.Equal("11144477735", result.Cpf.Value);

        // Verificar que foi salvo no banco
        var savedEntity = await _context.Customers.FindAsync(customer.Id);
        Assert.NotNull(savedEntity);
        Assert.Equal("New Customer", savedEntity.Name);
    }

    [Fact]
    public async Task AddAsync_WithCustomerWithEmail_ShouldSaveEmail()
    {
        // Arrange
        var customer = new Customer(
            id: Guid.NewGuid(),
            name: "Email Customer",
            email: new Email("email@test.com"),
            cpf: null,
            customerType: CustomerType.Registered
        );

        // Act
        var result = await _repository.AddAsync(customer);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Email);
        Assert.Equal("email@test.com", result.Email.Value);

        // Verificar no banco
        var savedEntity = await _context.Customers.FindAsync(customer.Id);
        Assert.NotNull(savedEntity);
        Assert.Equal("email@test.com", savedEntity.Email);
    }

    [Fact]
    public async Task AddAsync_WithCustomerWithCpf_ShouldSaveCpf()
    {
        // Arrange
        var customer = new Customer(
            id: Guid.NewGuid(),
            name: "CPF Customer",
            email: null,
            cpf: new Cpf("11144477735"),
            customerType: CustomerType.Registered
        );

        // Act
        var result = await _repository.AddAsync(customer);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Cpf);
        Assert.Equal("11144477735", result.Cpf.Value);

        // Verificar no banco
        var savedEntity = await _context.Customers.FindAsync(customer.Id);
        Assert.NotNull(savedEntity);
        Assert.Equal("11144477735", savedEntity.Cpf);
    }

    [Fact]
    public async Task AddAsync_WithAnonymousCustomer_ShouldSaveWithoutEmailAndCpf()
    {
        // Arrange
        var customer = new Customer(
            id: Guid.NewGuid(),
            name: null,
            email: null,
            cpf: null,
            customerType: CustomerType.Anonymous
        );

        // Act
        var result = await _repository.AddAsync(customer);

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.Email);
        Assert.Null(result.Cpf);
        Assert.Equal(CustomerType.Anonymous, result.CustomerType);

        // Verificar no banco
        var savedEntity = await _context.Customers.FindAsync(customer.Id);
        Assert.NotNull(savedEntity);
        Assert.Null(savedEntity.Email);
        Assert.Null(savedEntity.Cpf);
        Assert.Equal((int)CustomerType.Anonymous, savedEntity.CustomerType);
    }

    [Fact]
    public async Task AddAsync_ShouldMapDomainToEntityCorrectly()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customer = new Customer(
            id: customerId,
            name: "Mapped Customer",
            email: new Email("mapped@example.com"),
            cpf: new Cpf("11144477735"),
            customerType: CustomerType.Registered
        );

        // Act
        var result = await _repository.AddAsync(customer);

        // Assert
        Assert.NotNull(result);
        var savedEntity = await _context.Customers.FindAsync(customerId);
        Assert.NotNull(savedEntity);
        Assert.Equal(customerId, savedEntity.Id);
        Assert.Equal("Mapped Customer", savedEntity.Name);
        Assert.Equal("mapped@example.com", savedEntity.Email);
        Assert.Equal("11144477735", savedEntity.Cpf);
        Assert.Equal((int)CustomerType.Registered, savedEntity.CustomerType);
    }

    [Fact]
    public async Task AddAsync_ShouldSetCreatedAt()
    {
        // Arrange
        var customer = new Customer(
            id: Guid.NewGuid(),
            name: "Created At Customer",
            email: null,
            cpf: null,
            customerType: CustomerType.Registered
        );

        // Act
        var result = await _repository.AddAsync(customer);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.CreatedAt <= DateTime.UtcNow);
        Assert.True(result.CreatedAt >= DateTime.UtcNow.AddSeconds(-5));

        // Verificar no banco
        var savedEntity = await _context.Customers.FindAsync(customer.Id);
        Assert.NotNull(savedEntity);
        Assert.True(savedEntity.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public async Task AddAsync_ShouldReturnCustomerWithId()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customer = new Customer(
            id: customerId,
            name: "ID Customer",
            email: null,
            cpf: null,
            customerType: CustomerType.Registered
        );

        // Act
        var result = await _repository.AddAsync(customer);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(customerId, result.Id);
    }

    [Fact]
    public async Task AddAsync_WithRegisteredCustomer_ShouldSaveCustomerType()
    {
        // Arrange
        var customer = new Customer(
            id: Guid.NewGuid(),
            name: "Registered Customer",
            email: new Email("registered@example.com"),
            cpf: new Cpf("11144477735"),
            customerType: CustomerType.Registered
        );

        // Act
        var result = await _repository.AddAsync(customer);

        // Assert
        Assert.Equal(CustomerType.Registered, result.CustomerType);
        var savedEntity = await _context.Customers.FindAsync(customer.Id);
        Assert.NotNull(savedEntity);
        Assert.Equal((int)CustomerType.Registered, savedEntity.CustomerType);
    }

    [Fact]
    public async Task AddAsync_WithAnonymousCustomer_ShouldSaveCustomerType()
    {
        // Arrange
        var customer = new Customer(
            id: Guid.NewGuid(),
            name: null,
            email: null,
            cpf: null,
            customerType: CustomerType.Anonymous
        );

        // Act
        var result = await _repository.AddAsync(customer);

        // Assert
        Assert.Equal(CustomerType.Anonymous, result.CustomerType);
        var savedEntity = await _context.Customers.FindAsync(customer.Id);
        Assert.NotNull(savedEntity);
        Assert.Equal((int)CustomerType.Anonymous, savedEntity.CustomerType);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldMapCustomerTypeCorrectly()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var entity = new CustomerEntity
        {
            Id = customerId,
            Name = "Type Test Customer",
            CustomerType = (int)CustomerType.Registered,
            CreatedAt = DateTime.UtcNow
        };

        _context.Customers.Add(entity);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(customerId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(CustomerType.Registered, result.CustomerType);
    }

    public void Dispose()
    {
        _context?.Dispose();
        GC.SuppressFinalize(this);
    }
}

