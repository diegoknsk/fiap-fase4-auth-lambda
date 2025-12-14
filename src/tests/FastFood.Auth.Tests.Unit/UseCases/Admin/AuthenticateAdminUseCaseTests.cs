using Moq;
using FastFood.Auth.Application.Commands.Admin;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Application.UseCases.Admin;

namespace FastFood.Auth.Tests.Unit.UseCases.Admin;

/// <summary>
/// Testes unitários para AuthenticateAdminUseCase.
/// </summary>
public class AuthenticateAdminUseCaseTests
{
    private readonly Mock<ICognitoService> _cognitoServiceMock;
    private readonly AuthenticateAdminUseCase _useCase;

    public AuthenticateAdminUseCaseTests()
    {
        _cognitoServiceMock = new Mock<ICognitoService>();
        _useCase = new AuthenticateAdminUseCase(_cognitoServiceMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenCredentialsAreValid_ShouldReturnTokens()
    {
        // Arrange
        var username = "admin@example.com";
        var password = "SecurePassword123!";
        var command = new AuthenticateAdminCommand
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
        var result = await _useCase.ExecuteAsync(command);

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
        var command = new AuthenticateAdminCommand
        {
            Username = username,
            Password = password
        };

        _cognitoServiceMock
            .Setup(x => x.AuthenticateAsync(username, password))
            .ThrowsAsync(new UnauthorizedAccessException("Credenciais inválidas"));

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _useCase.ExecuteAsync(command));

        _cognitoServiceMock.Verify(x => x.AuthenticateAsync(username, password), Times.Once);
    }
}

