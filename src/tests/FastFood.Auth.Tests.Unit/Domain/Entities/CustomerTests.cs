using FastFood.Auth.Domain.Entities.CustomerIdentification;
using FastFood.Auth.Domain.Entities.CustomerIdentification.ValueObects;

namespace FastFood.Auth.Tests.Unit.Domain.Entities;

/// <summary>
/// Testes unitários para a entidade Customer.
/// </summary>
public class CustomerTests
{
    [Fact]
    public void Constructor_WithValidData_ShouldCreateCustomer()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "John Doe";
        var email = new Email("john@example.com");
        var cpf = new Cpf("11144477735");
        var customerType = CustomerType.Registered;

        // Act
        var customer = new Customer(id, name, email, cpf, customerType);

        // Assert
        Assert.NotNull(customer);
        Assert.Equal(id, customer.Id);
        Assert.Equal(name, customer.Name);
        Assert.Equal(email, customer.Email);
        Assert.Equal(cpf, customer.Cpf);
        Assert.Equal(customerType, customer.CustomerType);
    }

    [Fact]
    public void Constructor_WithRegisteredType_ShouldSetCustomerTypeRegistered()
    {
        // Arrange
        var id = Guid.NewGuid();
        var cpf = new Cpf("11144477735");

        // Act
        var customer = new Customer(id, null, null, cpf, CustomerType.Registered);

        // Assert
        Assert.Equal(CustomerType.Registered, customer.CustomerType);
    }

    [Fact]
    public void Constructor_WithAnonymousType_ShouldSetCustomerTypeAnonymous()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var customer = new Customer(id, null, null, null, CustomerType.Anonymous);

        // Assert
        Assert.Equal(CustomerType.Anonymous, customer.CustomerType);
    }

    [Fact]
    public void Constructor_ShouldSetCreatedAtToUtcNow()
    {
        // Arrange
        var id = Guid.NewGuid();
        var beforeCreation = DateTime.UtcNow;

        // Act
        var customer = new Customer(id, null, null, null, CustomerType.Anonymous);
        var afterCreation = DateTime.UtcNow;

        // Assert
        Assert.True(customer.CreatedAt >= beforeCreation);
        Assert.True(customer.CreatedAt <= afterCreation);
    }

    [Fact]
    public void AddCustomer_ShouldGenerateNewGuid()
    {
        // Arrange
        var id = Guid.NewGuid();
        var customer = new Customer(id, null, null, null, CustomerType.Anonymous);

        // Act
        customer.AddCustomer(null, null, null, CustomerType.Anonymous);

        // Assert
        Assert.NotEqual(Guid.Empty, customer.Id);
        Assert.NotEqual(id, customer.Id); // Deve gerar um novo Guid
    }

    [Fact]
    public void AddCustomer_ShouldSetCreatedAt()
    {
        // Arrange
        var id = Guid.NewGuid();
        var customer = new Customer(id, null, null, null, CustomerType.Anonymous);
        var beforeCreation = DateTime.UtcNow;

        // Act
        customer.AddCustomer(null, null, null, CustomerType.Anonymous);
        var afterCreation = DateTime.UtcNow;

        // Assert
        Assert.True(customer.CreatedAt >= beforeCreation);
        Assert.True(customer.CreatedAt <= afterCreation);
    }

    [Fact]
    public void AddCustomer_WithRegisteredType_ShouldCreateRegisteredCustomer()
    {
        // Arrange
        var id = Guid.NewGuid();
        var customer = new Customer(id, null, null, null, CustomerType.Anonymous);
        var cpf = new Cpf("11144477735");

        // Act
        customer.AddCustomer(null, null, cpf, CustomerType.Registered);

        // Assert
        Assert.Equal(CustomerType.Registered, customer.CustomerType);
        Assert.NotNull(customer.Cpf);
        Assert.Equal(cpf.Value, customer.Cpf.Value);
    }

    [Fact]
    public void AddCustomer_WithAnonymousType_ShouldCreateAnonymousCustomer()
    {
        // Arrange
        var id = Guid.NewGuid();
        var customer = new Customer(id, null, null, null, CustomerType.Registered);

        // Act
        customer.AddCustomer(null, null, null, CustomerType.Anonymous);

        // Assert
        Assert.Equal(CustomerType.Anonymous, customer.CustomerType);
        Assert.Null(customer.Cpf);
    }

    [Fact]
    public void Customer_WithNullCpf_ShouldBeValid()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var customer = new Customer(id, null, null, null, CustomerType.Anonymous);

        // Assert
        Assert.NotNull(customer);
        Assert.Null(customer.Cpf);
    }

    [Fact]
    public void Customer_WithNullEmail_ShouldBeValid()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var customer = new Customer(id, null, null, null, CustomerType.Anonymous);

        // Assert
        Assert.NotNull(customer);
        Assert.Null(customer.Email);
    }

    [Fact]
    public void Customer_WithNullName_ShouldBeValid()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var customer = new Customer(id, null, null, null, CustomerType.Anonymous);

        // Assert
        Assert.NotNull(customer);
        Assert.Null(customer.Name);
    }

    [Fact]
    public void AddCustomer_WithNameAndEmail_ShouldSetProperties()
    {
        // Arrange
        var id = Guid.NewGuid();
        var customer = new Customer(id, null, null, null, CustomerType.Anonymous);
        var name = "Jane Doe";
        var email = new Email("jane@example.com");
        var cpf = new Cpf("11144477735");

        // Act
        customer.AddCustomer(name, email, cpf, CustomerType.Registered);

        // Assert
        Assert.Equal(name, customer.Name);
        Assert.NotNull(customer.Email);
        Assert.Equal(email.Value, customer.Email.Value);
        Assert.NotNull(customer.Cpf);
        Assert.Equal(cpf.Value, customer.Cpf.Value);
        Assert.Equal(CustomerType.Registered, customer.CustomerType);
    }

    [Fact]
    public void AddCustomer_WithAllNullProperties_ShouldSetNullProperties()
    {
        // Arrange
        var id = Guid.NewGuid();
        var customer = new Customer(id, "Original Name", new Email("original@example.com"), new Cpf("11144477735"), CustomerType.Registered);

        // Act
        customer.AddCustomer(null, null, null, CustomerType.Anonymous);

        // Assert
        Assert.Null(customer.Name);
        Assert.Null(customer.Email);
        Assert.Null(customer.Cpf);
        Assert.Equal(CustomerType.Anonymous, customer.CustomerType);
    }

    [Fact]
    public void AddCustomer_WithEmailOnly_ShouldSetEmail()
    {
        // Arrange
        var id = Guid.NewGuid();
        var customer = new Customer(id, null, null, null, CustomerType.Anonymous);
        var email = new Email("test@example.com");

        // Act
        customer.AddCustomer(null, email, null, CustomerType.Registered);

        // Assert
        Assert.Null(customer.Name);
        Assert.NotNull(customer.Email);
        Assert.Equal(email.Value, customer.Email.Value);
        Assert.Null(customer.Cpf);
        Assert.Equal(CustomerType.Registered, customer.CustomerType);
    }

    [Fact]
    public void AddCustomer_WithCpfOnly_ShouldSetCpf()
    {
        // Arrange
        var id = Guid.NewGuid();
        var customer = new Customer(id, null, null, null, CustomerType.Anonymous);
        var cpf = new Cpf("11144477735");

        // Act
        customer.AddCustomer(null, null, cpf, CustomerType.Registered);

        // Assert
        Assert.Null(customer.Name);
        Assert.Null(customer.Email);
        Assert.NotNull(customer.Cpf);
        Assert.Equal(cpf.Value, customer.Cpf.Value);
        Assert.Equal(CustomerType.Registered, customer.CustomerType);
    }

    [Fact]
    public void AddCustomer_WithNameOnly_ShouldSetName()
    {
        // Arrange
        var id = Guid.NewGuid();
        var customer = new Customer(id, null, null, null, CustomerType.Anonymous);
        var name = "Test Customer";

        // Act
        customer.AddCustomer(name, null, null, CustomerType.Registered);

        // Assert
        Assert.Equal(name, customer.Name);
        Assert.Null(customer.Email);
        Assert.Null(customer.Cpf);
        Assert.Equal(CustomerType.Registered, customer.CustomerType);
    }
}

