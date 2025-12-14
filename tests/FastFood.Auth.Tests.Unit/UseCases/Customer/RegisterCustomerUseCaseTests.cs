using Moq;
using FastFood.Auth.Application.Commands.Customer;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Application.UseCases.Customer;
using FastFood.Auth.Domain.Entities.CustomerIdentification;
using FastFood.Auth.Domain.Entities.CustomerIdentification.ValueObects;
using DomainCustomer = FastFood.Auth.Domain.Entities.CustomerIdentification.Customer;

namespace FastFood.Auth.Tests.Unit.UseCases.Customer;

/// <summary>
/// Testes unitários para RegisterCustomerUseCase.
/// </summary>
public class RegisterCustomerUseCaseTests
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly RegisterCustomerUseCase _useCase;

    public RegisterCustomerUseCaseTests()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _tokenServiceMock = new Mock<ITokenService>();
        _useCase = new RegisterCustomerUseCase(
            _customerRepositoryMock.Object,
            _tokenServiceMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenCustomerExists_ShouldReturnExistingCustomerToken()
    {
        // Arrange
        var cpf = "11144477735"; // CPF válido para testes
        var existingCustomerId = Guid.NewGuid();
        var existingCustomer = new DomainCustomer(
            existingCustomerId,
            null,
            null,
            new Cpf(cpf),
            CustomerTypeEnum.Registered);

        var command = new RegisterCustomerCommand { Cpf = cpf };
        var expectedToken = "existing-token";
        var expectedExpiresAt = DateTime.UtcNow.AddHours(1);

        _customerRepositoryMock
            .Setup(x => x.GetByCpfAsync(cpf))
            .ReturnsAsync((DomainCustomer?)existingCustomer);

        _tokenServiceMock
            .Setup(x => x.GenerateToken(existingCustomerId, out It.Ref<DateTime>.IsAny))
            .Callback((Guid id, out DateTime expiresAt) => { expiresAt = expectedExpiresAt; })
            .Returns(expectedToken);

        // Act
        var result = await _useCase.ExecuteAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedToken, result.Token);
        Assert.Equal(existingCustomerId, result.CustomerId);
        Assert.Equal(expectedExpiresAt, result.ExpiresAt);

        _customerRepositoryMock.Verify(x => x.GetByCpfAsync(cpf), Times.Once);
        _customerRepositoryMock.Verify(x => x.AddAsync(It.IsAny<DomainCustomer>()), Times.Never);
        _tokenServiceMock.Verify(x => x.GenerateToken(existingCustomerId, out It.Ref<DateTime>.IsAny), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenCustomerNotExists_ShouldCreateNewCustomerAndReturnToken()
    {
        // Arrange
        var cpf = "11144477735"; // CPF válido para testes
        var newCustomerId = Guid.NewGuid();
        var command = new RegisterCustomerCommand { Cpf = cpf };
        var expectedToken = "new-token";
        var expectedExpiresAt = DateTime.UtcNow.AddHours(1);

        _customerRepositoryMock
            .Setup(x => x.GetByCpfAsync(cpf))
            .ReturnsAsync((DomainCustomer?)null);

        DomainCustomer? savedCustomer = null;
        _customerRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<DomainCustomer>()))
            .Callback<DomainCustomer>(c => { savedCustomer = c; })
            .ReturnsAsync((DomainCustomer c) => c);

        _tokenServiceMock
            .Setup(x => x.GenerateToken(It.IsAny<Guid>(), out It.Ref<DateTime>.IsAny))
            .Callback((Guid id, out DateTime expiresAt) => { expiresAt = expectedExpiresAt; })
            .Returns(expectedToken);

        // Act
        var result = await _useCase.ExecuteAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedToken, result.Token);
        Assert.NotNull(savedCustomer);
        Assert.Equal(CustomerTypeEnum.Registered, savedCustomer.CustomerType);
        Assert.NotNull(savedCustomer.Cpf);
        Assert.Equal(cpf, savedCustomer.Cpf.Value);
        Assert.Equal(savedCustomer.Id, result.CustomerId);
        Assert.Equal(expectedExpiresAt, result.ExpiresAt);

        _customerRepositoryMock.Verify(x => x.GetByCpfAsync(cpf), Times.Once);
        _customerRepositoryMock.Verify(x => x.AddAsync(It.Is<DomainCustomer>(c => 
            c.CustomerType == CustomerTypeEnum.Registered && 
            c.Cpf != null && 
            c.Cpf.Value == cpf)), Times.Once);
        _tokenServiceMock.Verify(x => x.GenerateToken(It.IsAny<Guid>(), out It.Ref<DateTime>.IsAny), Times.Once);
    }
}

