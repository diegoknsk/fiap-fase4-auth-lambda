using Microsoft.AspNetCore.Mvc;
using Moq;
using FastFood.Auth.Application.UseCases.Admin;
using FastFood.Auth.Application.OutputModels.Admin;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Application.Presenters.Admin;
using FastFood.Auth.Lambda.Controllers;
using FastFood.Auth.Lambda.Models.Admin;

namespace FastFood.Auth.Tests.Unit.Controllers;

/// <summary>
/// Testes unitários para AdminController.
/// </summary>
public class AdminControllerTests
{
    private readonly Mock<ICognitoService> _cognitoServiceMock;
    private readonly AuthenticateAdminUseCase _authenticateUseCase;
    private readonly AuthenticateAdminPresenter _authenticatePresenter;
    private readonly AdminController _controller;

    public AdminControllerTests()
    {
        _cognitoServiceMock = new Mock<ICognitoService>();
        _authenticateUseCase = new AuthenticateAdminUseCase(_cognitoServiceMock.Object);
        _authenticatePresenter = new AuthenticateAdminPresenter();
        _controller = new AdminController(_authenticateUseCase, _authenticatePresenter);
    }

    [Fact]
    public async Task PostLogin_WithValidCredentials_ShouldReturnOkWithTokens()
    {
        // Arrange
        var request = new AdminLoginRequest
        {
            Username = "admin@example.com",
            Password = "SecurePassword123!"
        };

        var expectedResult = new AuthenticateAdminResult
        {
            AccessToken = "access-token-123",
            IdToken = "id-token-456",
            ExpiresIn = 3600,
            TokenType = "Bearer"
        };

        _cognitoServiceMock
            .Setup(x => x.AuthenticateAsync(request.Username, request.Password))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.Login(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<AuthenticateAdminOutputModel>(okResult.Value);
        Assert.Equal(expectedResult.AccessToken, response.AccessToken);
        Assert.Equal(expectedResult.IdToken, response.IdToken);
        Assert.Equal(expectedResult.ExpiresIn, response.ExpiresIn);
        Assert.Equal(expectedResult.TokenType, response.TokenType);
    }

    [Fact]
    public async Task PostLogin_WithInvalidCredentials_ShouldReturnUnauthorized()
    {
        // Arrange
        var request = new AdminLoginRequest
        {
            Username = "admin@example.com",
            Password = "WrongPassword"
        };

        _cognitoServiceMock
            .Setup(x => x.AuthenticateAsync(request.Username, request.Password))
            .ThrowsAsync(new UnauthorizedAccessException("Credenciais inválidas"));

        // Act
        var result = await _controller.Login(request);

        // Assert
        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.NotNull(unauthorizedResult.Value);
    }

    [Fact]
    public async Task PostLogin_ShouldCallUseCase()
    {
        // Arrange
        var request = new AdminLoginRequest
        {
            Username = "admin@example.com",
            Password = "SecurePassword123!"
        };

        var expectedResult = new AuthenticateAdminResult
        {
            AccessToken = "access-token-123",
            IdToken = "id-token-456",
            ExpiresIn = 3600,
            TokenType = "Bearer"
        };

        _cognitoServiceMock
            .Setup(x => x.AuthenticateAsync(request.Username, request.Password))
            .ReturnsAsync(expectedResult);

        // Act
        await _controller.Login(request);

        // Assert
        _cognitoServiceMock.Verify(
            x => x.AuthenticateAsync(request.Username, request.Password), 
            Times.Once);
    }
}

