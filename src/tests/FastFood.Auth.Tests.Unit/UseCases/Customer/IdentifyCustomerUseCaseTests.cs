using Moq;
using FastFood.Auth.Application.InputModels.Customer;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Application.UseCases.Customer;
using FastFood.Auth.Application.Presenters.Customer;
using FastFood.Auth.Domain.Entities.CustomerIdentification;
using FastFood.Auth.Domain.Entities.CustomerIdentification.ValueObects;
using FastFood.Auth.Domain.Exceptions;
using DomainCustomer = FastFood.Auth.Domain.Entities.CustomerIdentification.Customer;

namespace FastFood.Auth.Tests.Unit.UseCases.Customer;

/// <summary>
/// Testes unitários para IdentifyCustomerUseCase.
/// </summary>
public class IdentifyCustomerUseCaseTests
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly IdentifyCustomerUseCase _useCase;

    public IdentifyCustomerUseCaseTests()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _tokenServiceMock = new Mock<ITokenService>();
        _useCase = new IdentifyCustomerUseCase(
            _customerRepositoryMock.Object,
            _tokenServiceMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenCustomerExists_ShouldReturnToken()
    {
        // Arrange
        var cpf = "11144477735"; // CPF válido para testes
        var customerId = Guid.NewGuid();
        var existingCustomer = new DomainCustomer(
            customerId,
            null,
            null,
            new Cpf(cpf),
            CustomerType.Registered);

        var inputModel = new IdentifyCustomerInputModel { Cpf = cpf };
        var expectedToken = "test-token";
        var expectedExpiresAt = DateTime.UtcNow.AddHours(1);

        _customerRepositoryMock
            .Setup(x => x.GetByCpfAsync(cpf))
            .ReturnsAsync((DomainCustomer?)existingCustomer);

        _tokenServiceMock
            .Setup(x => x.GenerateToken(customerId, out It.Ref<DateTime>.IsAny))
            .Callback((Guid id, out DateTime expiresAt) => { expiresAt = expectedExpiresAt; })
            .Returns(expectedToken);

        // Act
        var result = await _useCase.ExecuteAsync(inputModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedToken, result.Token);
        Assert.Equal(customerId, result.CustomerId);
        Assert.Equal(expectedExpiresAt, result.ExpiresAt);

        _customerRepositoryMock.Verify(x => x.GetByCpfAsync(cpf), Times.Once);
        _tokenServiceMock.Verify(x => x.GenerateToken(customerId, out It.Ref<DateTime>.IsAny), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenCustomerNotExists_ShouldThrowUnauthorizedAccessException()
    {
        // Arrange
        var cpf = "11144477735"; // CPF válido para testes
        var inputModel = new IdentifyCustomerInputModel { Cpf = cpf };

        _customerRepositoryMock
            .Setup(x => x.GetByCpfAsync(cpf))
            .ReturnsAsync((DomainCustomer?)null);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            await _useCase.ExecuteAsync(inputModel));

        _customerRepositoryMock.Verify(x => x.GetByCpfAsync(cpf), Times.Once);
        _tokenServiceMock.Verify(x => x.GenerateToken(It.IsAny<Guid>(), out It.Ref<DateTime>.IsAny), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldValidateCpf()
    {
        // Arrange
        var invalidCpf = "12345678901"; // CPF inválido
        var inputModel = new IdentifyCustomerInputModel { Cpf = invalidCpf };

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(async () =>
            await _useCase.ExecuteAsync(inputModel));

        _customerRepositoryMock.Verify(x => x.GetByCpfAsync(It.IsAny<string>()), Times.Never);
        _tokenServiceMock.Verify(x => x.GenerateToken(It.IsAny<Guid>(), out It.Ref<DateTime>.IsAny), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldCallRepositoryGetByCpfAsync()
    {
        // Arrange
        var cpf = "11144477735";
        var customerId = Guid.NewGuid();
        var existingCustomer = new DomainCustomer(
            customerId,
            null,
            null,
            new Cpf(cpf),
            CustomerType.Registered);

        var inputModel = new IdentifyCustomerInputModel { Cpf = cpf };
        var expectedToken = "test-token";
        var expectedExpiresAt = DateTime.UtcNow.AddHours(1);

        _customerRepositoryMock
            .Setup(x => x.GetByCpfAsync(cpf))
            .ReturnsAsync((DomainCustomer?)existingCustomer);

        _tokenServiceMock
            .Setup(x => x.GenerateToken(customerId, out It.Ref<DateTime>.IsAny))
            .Callback((Guid id, out DateTime expiresAt) => { expiresAt = expectedExpiresAt; })
            .Returns(expectedToken);

        // Act
        await _useCase.ExecuteAsync(inputModel);

        // Assert
        _customerRepositoryMock.Verify(x => x.GetByCpfAsync(cpf), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldCallTokenServiceWhenCustomerFound()
    {
        // Arrange
        var cpf = "11144477735";
        var customerId = Guid.NewGuid();
        var existingCustomer = new DomainCustomer(
            customerId,
            null,
            null,
            new Cpf(cpf),
            CustomerType.Registered);

        var inputModel = new IdentifyCustomerInputModel { Cpf = cpf };
        var expectedToken = "test-token";
        var expectedExpiresAt = DateTime.UtcNow.AddHours(1);

        _customerRepositoryMock
            .Setup(x => x.GetByCpfAsync(cpf))
            .ReturnsAsync((DomainCustomer?)existingCustomer);

        _tokenServiceMock
            .Setup(x => x.GenerateToken(customerId, out It.Ref<DateTime>.IsAny))
            .Callback((Guid id, out DateTime expiresAt) => { expiresAt = expectedExpiresAt; })
            .Returns(expectedToken);

        // Act
        await _useCase.ExecuteAsync(inputModel);

        // Assert
        _tokenServiceMock.Verify(x => x.GenerateToken(customerId, out It.Ref<DateTime>.IsAny), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WithNonExistingCustomer_ShouldThrowUnauthorizedAccessExceptionWithCorrectMessage()
    {
        // Arrange
        var cpf = "11144477735";
        var inputModel = new IdentifyCustomerInputModel { Cpf = cpf };

        _customerRepositoryMock
            .Setup(x => x.GetByCpfAsync(cpf))
            .ReturnsAsync((DomainCustomer?)null);

        // Act
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _useCase.ExecuteAsync(inputModel));

        // Assert
        Assert.Equal("Customer not found with the provided CPF.", exception.Message);
        _tokenServiceMock.Verify(
            t => t.GenerateToken(It.IsAny<Guid>(), out It.Ref<DateTime>.IsAny),
            Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WithNullCpf_ShouldThrowDomainException()
    {
        // Arrange
        var inputModel = new IdentifyCustomerInputModel { Cpf = null! };

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() =>
            _useCase.ExecuteAsync(inputModel));

        _customerRepositoryMock.Verify(x => x.GetByCpfAsync(It.IsAny<string>()), Times.Never);
        _tokenServiceMock.Verify(
            t => t.GenerateToken(It.IsAny<Guid>(), out It.Ref<DateTime>.IsAny),
            Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WithEmptyCpf_ShouldThrowDomainException()
    {
        // Arrange
        var inputModel = new IdentifyCustomerInputModel { Cpf = string.Empty };

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() =>
            _useCase.ExecuteAsync(inputModel));

        _customerRepositoryMock.Verify(x => x.GetByCpfAsync(It.IsAny<string>()), Times.Never);
        _tokenServiceMock.Verify(
            t => t.GenerateToken(It.IsAny<Guid>(), out It.Ref<DateTime>.IsAny),
            Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenRepositoryFails_ShouldPropagateException()
    {
        // Arrange
        var cpf = "11144477735";
        var inputModel = new IdentifyCustomerInputModel { Cpf = cpf };

        _customerRepositoryMock
            .Setup(x => x.GetByCpfAsync(cpf))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _useCase.ExecuteAsync(inputModel));

        _tokenServiceMock.Verify(
            t => t.GenerateToken(It.IsAny<Guid>(), out It.Ref<DateTime>.IsAny),
            Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenTokenServiceFails_ShouldPropagateException()
    {
        // Arrange
        var cpf = "11144477735";
        var inputModel = new IdentifyCustomerInputModel { Cpf = cpf };
        var customerId = Guid.NewGuid();
        var existingCustomer = new DomainCustomer(
            customerId,
            null,
            null,
            new Cpf(cpf),
            CustomerType.Registered);

        _customerRepositoryMock
            .Setup(x => x.GetByCpfAsync(cpf))
            .ReturnsAsync((DomainCustomer?)existingCustomer);

        _tokenServiceMock
            .Setup(x => x.GenerateToken(customerId, out It.Ref<DateTime>.IsAny))
            .Throws(new InvalidOperationException("Token generation failed"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _useCase.ExecuteAsync(inputModel));

        _customerRepositoryMock.Verify(x => x.GetByCpfAsync(cpf), Times.Once);
    }
}

