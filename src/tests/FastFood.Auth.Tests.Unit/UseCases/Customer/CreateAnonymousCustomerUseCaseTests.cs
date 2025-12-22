using Moq;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Application.UseCases.Customer;
using FastFood.Auth.Application.Presenters.Customer;
using FastFood.Auth.Domain.Entities.CustomerIdentification;
using DomainCustomer = FastFood.Auth.Domain.Entities.CustomerIdentification.Customer;

namespace FastFood.Auth.Tests.Unit.UseCases.Customer;

/// <summary>
/// Testes unitários para CreateAnonymousCustomerUseCase.
/// </summary>
public class CreateAnonymousCustomerUseCaseTests
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly CreateAnonymousCustomerUseCase _useCase;

    public CreateAnonymousCustomerUseCaseTests()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _tokenServiceMock = new Mock<ITokenService>();
        _useCase = new CreateAnonymousCustomerUseCase(
            _customerRepositoryMock.Object,
            _tokenServiceMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldCreateAnonymousCustomer_AndReturnToken()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var expectedToken = "test-token";
        var expectedExpiresAt = DateTime.UtcNow.AddHours(1);

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
        var result = await _useCase.ExecuteAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedToken, result.Token);
        Assert.NotEqual(Guid.Empty, result.CustomerId);
        Assert.Equal(expectedExpiresAt, result.ExpiresAt);
        Assert.NotNull(savedCustomer);
        Assert.Equal(CustomerType.Anonymous, savedCustomer.CustomerType);
        Assert.Null(savedCustomer.Cpf);
        Assert.Null(savedCustomer.Email);
        Assert.Null(savedCustomer.Name);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldCallRepositoryAddAsync()
    {
        // Arrange
        var expectedToken = "test-token";
        var expectedExpiresAt = DateTime.UtcNow.AddHours(1);

        _customerRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<DomainCustomer>()))
            .ReturnsAsync((DomainCustomer c) => c);

        _tokenServiceMock
            .Setup(x => x.GenerateToken(It.IsAny<Guid>(), out It.Ref<DateTime>.IsAny))
            .Callback((Guid id, out DateTime expiresAt) => { expiresAt = expectedExpiresAt; })
            .Returns(expectedToken);

        // Act
        await _useCase.ExecuteAsync();

        // Assert
        _customerRepositoryMock.Verify(
            x => x.AddAsync(It.Is<DomainCustomer>(c => 
                c.CustomerType == CustomerType.Anonymous && 
                c.Cpf == null && 
                c.Email == null && 
                c.Name == null)), 
            Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldCallTokenServiceGenerateToken()
    {
        // Arrange
        var expectedToken = "test-token";
        var expectedExpiresAt = DateTime.UtcNow.AddHours(1);

        _customerRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<DomainCustomer>()))
            .ReturnsAsync((DomainCustomer c) => c);

        _tokenServiceMock
            .Setup(x => x.GenerateToken(It.IsAny<Guid>(), out It.Ref<DateTime>.IsAny))
            .Callback((Guid id, out DateTime expiresAt) => { expiresAt = expectedExpiresAt; })
            .Returns(expectedToken);

        // Act
        await _useCase.ExecuteAsync();

        // Assert
        _tokenServiceMock.Verify(
            x => x.GenerateToken(It.IsAny<Guid>(), out It.Ref<DateTime>.IsAny), 
            Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnResponseWithTokenAndCustomerId()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var expectedToken = "test-token";
        var expectedExpiresAt = DateTime.UtcNow.AddHours(1);

        _customerRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<DomainCustomer>()))
            .ReturnsAsync((DomainCustomer c) => c);

        _tokenServiceMock
            .Setup(x => x.GenerateToken(It.IsAny<Guid>(), out It.Ref<DateTime>.IsAny))
            .Callback((Guid id, out DateTime expiresAt) => { expiresAt = expectedExpiresAt; })
            .Returns(expectedToken);

        // Act
        var result = await _useCase.ExecuteAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedToken, result.Token);
        Assert.NotEqual(Guid.Empty, result.CustomerId);
        Assert.Equal(expectedExpiresAt, result.ExpiresAt);
    }

    [Fact]
    public async Task ExecuteAsync_WhenRepositoryFails_ShouldPropagateException()
    {
        // Arrange
        _customerRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<DomainCustomer>()))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _useCase.ExecuteAsync());

        _tokenServiceMock.Verify(
            t => t.GenerateToken(It.IsAny<Guid>(), out It.Ref<DateTime>.IsAny),
            Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenTokenServiceFails_ShouldPropagateException()
    {
        // Arrange
        _customerRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<DomainCustomer>()))
            .ReturnsAsync((DomainCustomer c) => c);

        _tokenServiceMock
            .Setup(x => x.GenerateToken(It.IsAny<Guid>(), out It.Ref<DateTime>.IsAny))
            .Throws(new InvalidOperationException("Token generation failed"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _useCase.ExecuteAsync());

        _customerRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<DomainCustomer>()),
            Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenRepositoryThrowsException_ShouldNotCreateCustomer()
    {
        // Arrange
        _customerRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<DomainCustomer>()))
            .ThrowsAsync(new Exception("Repository error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() =>
            _useCase.ExecuteAsync());

        _tokenServiceMock.Verify(
            t => t.GenerateToken(It.IsAny<Guid>(), out It.Ref<DateTime>.IsAny),
            Times.Never);
    }
}

