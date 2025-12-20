using Moq;
using FastFood.Auth.Application.InputModels.Admin;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Application.UseCases.Admin;
using FastFood.Auth.Application.Presenters.Admin;

namespace FastFood.Auth.Tests.Unit.UseCases.Admin;

/// <summary>
/// Testes unitários para AuthenticateAdminUseCase.
/// </summary>
public class AuthenticateAdminUseCaseTests
{
    private readonly Mock<ICognitoService> _cognitoServiceMock;
    private readonly AuthenticateAdminPresenter _presenter;
    private readonly AuthenticateAdminUseCase _useCase;

    public AuthenticateAdminUseCaseTests()
    {
        _cognitoServiceMock = new Mock<ICognitoService>();
        _presenter = new AuthenticateAdminPresenter();
        _useCase = new AuthenticateAdminUseCase(_cognitoServiceMock.Object, _presenter);
    }

    [Fact]
    public async Task ExecuteAsync_WhenCredentialsAreValid_ShouldReturnTokens()
    {
        // Arrange
        var username = "admin@example.com";
        var password = "SecurePassword123!";
        var inputModel = new AuthenticateAdminInputModel
        {
            Username = username,
            Password = password
        };

        var expectedResult = new AuthenticateAdminResult
        {
            AccessToken = "access-token-123",
            IdToken = "id-token-456",
            ExpiresIn = 3600,
            TokenType = "Bearer"
        };

        _cognitoServiceMock
            .Setup(x => x.AuthenticateAsync(username, password))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _useCase.ExecuteAsync(inputModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResult.AccessToken, result.AccessToken);
        Assert.Equal(expectedResult.IdToken, result.IdToken);
        Assert.Equal(expectedResult.ExpiresIn, result.ExpiresIn);
        Assert.Equal(expectedResult.TokenType, result.TokenType);

        _cognitoServiceMock.Verify(x => x.AuthenticateAsync(username, password), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenCredentialsAreInvalid_ShouldThrowUnauthorizedAccessException()
    {
        // Arrange
        var username = "admin@example.com";
        var password = "WrongPassword";
        var inputModel = new AuthenticateAdminInputModel
        {
            Username = username,
            Password = password
        };

        _cognitoServiceMock
            .Setup(x => x.AuthenticateAsync(username, password))
            .ThrowsAsync(new UnauthorizedAccessException("Credenciais inválidas"));

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _useCase.ExecuteAsync(inputModel));

        _cognitoServiceMock.Verify(x => x.AuthenticateAsync(username, password), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldCallCognitoServiceAuthenticateAsync()
    {
        // Arrange
        var username = "admin@example.com";
        var password = "SecurePassword123!";
        var inputModel = new AuthenticateAdminInputModel
        {
            Username = username,
            Password = password
        };

        var expectedResult = new AuthenticateAdminResult
        {
            AccessToken = "access-token-123",
            IdToken = "id-token-456",
            ExpiresIn = 3600,
            TokenType = "Bearer"
        };

        _cognitoServiceMock
            .Setup(x => x.AuthenticateAsync(username, password))
            .ReturnsAsync(expectedResult);

        // Act
        await _useCase.ExecuteAsync(inputModel);

        // Assert
        _cognitoServiceMock.Verify(x => x.AuthenticateAsync(username, password), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenCredentialsValid_ShouldReturnAccessTokenAndIdToken()
    {
        // Arrange
        var username = "admin@example.com";
        var password = "SecurePassword123!";
        var inputModel = new AuthenticateAdminInputModel
        {
            Username = username,
            Password = password
        };

        var expectedResult = new AuthenticateAdminResult
        {
            AccessToken = "access-token-123",
            IdToken = "id-token-456",
            ExpiresIn = 3600,
            TokenType = "Bearer"
        };

        _cognitoServiceMock
            .Setup(x => x.AuthenticateAsync(username, password))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _useCase.ExecuteAsync(inputModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("access-token-123", result.AccessToken);
        Assert.Equal("id-token-456", result.IdToken);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldPassUsernameAndPasswordToCognitoService()
    {
        // Arrange
        var username = "admin@example.com";
        var password = "SecurePassword123!";
        var inputModel = new AuthenticateAdminInputModel
        {
            Username = username,
            Password = password
        };

        var expectedResult = new AuthenticateAdminResult
        {
            AccessToken = "access-token-123",
            IdToken = "id-token-456",
            ExpiresIn = 3600,
            TokenType = "Bearer"
        };

        _cognitoServiceMock
            .Setup(x => x.AuthenticateAsync(username, password))
            .ReturnsAsync(expectedResult);

        // Act
        await _useCase.ExecuteAsync(inputModel);

        // Assert
        _cognitoServiceMock.Verify(
            x => x.AuthenticateAsync(
                It.Is<string>(u => u == username),
                It.Is<string>(p => p == password)),
            Times.Once);
    }
}

