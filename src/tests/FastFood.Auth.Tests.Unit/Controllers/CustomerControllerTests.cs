using Microsoft.AspNetCore.Mvc;
using Moq;
using FastFood.Auth.Application.UseCases.Customer;
using FastFood.Auth.Application.Responses.Customer;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Lambda.Controllers;
using FastFood.Auth.Lambda.Models.Customer;
using FastFood.Auth.Domain.Entities.CustomerIdentification;
using FastFood.Auth.Domain.Entities.CustomerIdentification.ValueObects;
using DomainCustomer = FastFood.Auth.Domain.Entities.CustomerIdentification.Customer;

namespace FastFood.Auth.Tests.Unit.Controllers;

/// <summary>
/// Testes unitários para CustomerController.
/// </summary>
public class CustomerControllerTests
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly CreateAnonymousCustomerUseCase _createAnonymousUseCase;
    private readonly RegisterCustomerUseCase _registerUseCase;
    private readonly IdentifyCustomerUseCase _identifyUseCase;
    private readonly CustomerController _controller;

    public CustomerControllerTests()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _tokenServiceMock = new Mock<ITokenService>();
        _createAnonymousUseCase = new CreateAnonymousCustomerUseCase(
            _customerRepositoryMock.Object,
            _tokenServiceMock.Object);
        _registerUseCase = new RegisterCustomerUseCase(
            _customerRepositoryMock.Object,
            _tokenServiceMock.Object);
        _identifyUseCase = new IdentifyCustomerUseCase(
            _customerRepositoryMock.Object,
            _tokenServiceMock.Object);
        _controller = new CustomerController(
            _createAnonymousUseCase,
            _registerUseCase,
            _identifyUseCase);
    }

    [Fact]
    public async Task PostAnonymous_ShouldReturnOkWithToken()
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
        var result = await _controller.CreateAnonymous();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<CreateAnonymousCustomerResponse>(okResult.Value);
        Assert.Equal(expectedToken, response.Token);
        Assert.NotEqual(Guid.Empty, response.CustomerId);
        Assert.Equal(expectedExpiresAt, response.ExpiresAt);
    }

    [Fact]
    public async Task PostAnonymous_ShouldCallUseCase()
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
        await _controller.CreateAnonymous();

        // Assert
        _customerRepositoryMock.Verify(x => x.AddAsync(It.IsAny<DomainCustomer>()), Times.Once);
        _tokenServiceMock.Verify(x => x.GenerateToken(It.IsAny<Guid>(), out It.Ref<DateTime>.IsAny), Times.Once);
    }

    [Fact]
    public async Task PostRegister_WithValidCpf_ShouldReturnOkWithToken()
    {
        // Arrange
        var request = new RegisterCustomerRequest { Cpf = "11144477735" };
        var cpf = "11144477735";
        var expectedToken = "test-token";
        var expectedExpiresAt = DateTime.UtcNow.AddHours(1);

        _customerRepositoryMock
            .Setup(x => x.GetByCpfAsync(cpf))
            .ReturnsAsync((DomainCustomer?)null);

        _customerRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<DomainCustomer>()))
            .ReturnsAsync((DomainCustomer c) => c);

        _tokenServiceMock
            .Setup(x => x.GenerateToken(It.IsAny<Guid>(), out It.Ref<DateTime>.IsAny))
            .Callback((Guid id, out DateTime expiresAt) => { expiresAt = expectedExpiresAt; })
            .Returns(expectedToken);

        // Act
        var result = await _controller.Register(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<RegisterCustomerResponse>(okResult.Value);
        Assert.Equal(expectedToken, response.Token);
        Assert.NotEqual(Guid.Empty, response.CustomerId);
        Assert.Equal(expectedExpiresAt, response.ExpiresAt);
    }

    [Fact]
    public async Task PostRegister_ShouldCallUseCase()
    {
        // Arrange
        var request = new RegisterCustomerRequest { Cpf = "11144477735" };
        var cpf = "11144477735";
        var expectedToken = "test-token";
        var expectedExpiresAt = DateTime.UtcNow.AddHours(1);

        _customerRepositoryMock
            .Setup(x => x.GetByCpfAsync(cpf))
            .ReturnsAsync((DomainCustomer?)null);

        _customerRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<DomainCustomer>()))
            .ReturnsAsync((DomainCustomer c) => c);

        _tokenServiceMock
            .Setup(x => x.GenerateToken(It.IsAny<Guid>(), out It.Ref<DateTime>.IsAny))
            .Callback((Guid id, out DateTime expiresAt) => { expiresAt = expectedExpiresAt; })
            .Returns(expectedToken);

        // Act
        await _controller.Register(request);

        // Assert
        _customerRepositoryMock.Verify(x => x.GetByCpfAsync(cpf), Times.Once);
        _customerRepositoryMock.Verify(x => x.AddAsync(It.IsAny<DomainCustomer>()), Times.Once);
        _tokenServiceMock.Verify(x => x.GenerateToken(It.IsAny<Guid>(), out It.Ref<DateTime>.IsAny), Times.Once);
    }

    [Fact]
    public async Task PostIdentify_WithValidCpf_ShouldReturnOkWithToken()
    {
        // Arrange
        var request = new IdentifyCustomerRequest { Cpf = "11144477735" };
        var cpf = "11144477735";
        var customerId = Guid.NewGuid();
        var existingCustomer = new DomainCustomer(
            customerId,
            null,
            null,
            new Cpf(cpf),
            CustomerTypeEnum.Registered);
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
        var result = await _controller.Identify(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<IdentifyCustomerResponse>(okResult.Value);
        Assert.Equal(expectedToken, response.Token);
        Assert.Equal(customerId, response.CustomerId);
        Assert.Equal(expectedExpiresAt, response.ExpiresAt);
    }

    [Fact]
    public async Task PostIdentify_WithInvalidCpf_ShouldReturnUnauthorized()
    {
        // Arrange
        var request = new IdentifyCustomerRequest { Cpf = "11144477735" };
        var cpf = "11144477735";

        _customerRepositoryMock
            .Setup(x => x.GetByCpfAsync(cpf))
            .ReturnsAsync((DomainCustomer?)null);

        // Act
        var result = await _controller.Identify(request);

        // Assert
        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.NotNull(unauthorizedResult.Value);
    }

    [Fact]
    public async Task PostIdentify_ShouldCallUseCase()
    {
        // Arrange
        var request = new IdentifyCustomerRequest { Cpf = "11144477735" };
        var cpf = "11144477735";
        var customerId = Guid.NewGuid();
        var existingCustomer = new DomainCustomer(
            customerId,
            null,
            null,
            new Cpf(cpf),
            CustomerTypeEnum.Registered);
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
        await _controller.Identify(request);

        // Assert
        _customerRepositoryMock.Verify(x => x.GetByCpfAsync(cpf), Times.Once);
        _tokenServiceMock.Verify(x => x.GenerateToken(customerId, out It.Ref<DateTime>.IsAny), Times.Once);
    }
}

