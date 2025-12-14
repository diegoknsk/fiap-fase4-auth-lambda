using Moq;
using FastFood.Auth.Application.Commands.Customer;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Application.UseCases.Customer;
using FastFood.Auth.Domain.Entities.CustomerIdentification;
using FastFood.Auth.Domain.Entities.CustomerIdentification.ValueObects;
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
            CustomerTypeEnum.Registered);

        var command = new IdentifyCustomerCommand { Cpf = cpf };
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
        var result = await _useCase.ExecuteAsync(command);

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
        var command = new IdentifyCustomerCommand { Cpf = cpf };

        _customerRepositoryMock
            .Setup(x => x.GetByCpfAsync(cpf))
            .ReturnsAsync((DomainCustomer?)null);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            await _useCase.ExecuteAsync(command));

        _customerRepositoryMock.Verify(x => x.GetByCpfAsync(cpf), Times.Once);
        _tokenServiceMock.Verify(x => x.GenerateToken(It.IsAny<Guid>(), out It.Ref<DateTime>.IsAny), Times.Never);
    }
}

