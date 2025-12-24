using Microsoft.AspNetCore.Mvc;
using Moq;
using FastFood.Auth.Application.UseCases.Admin;
using FastFood.Auth.Application.InputModels.Admin;
using FastFood.Auth.Application.OutputModels.Admin;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Application.Presenters.Admin;
using FastFood.Auth.Lambda.Admin.Controllers;

namespace FastFood.Auth.Tests.Unit.Controllers;

/// <summary>
/// Testes unitários para AdminController.
/// </summary>
public class AdminControllerTests
{
    private readonly Mock<ICognitoService> _cognitoServiceMock;
    private readonly AuthenticateAdminUseCase _authenticateUseCase;
    private readonly AdminController _controller;

    public AdminControllerTests()
    {
        _cognitoServiceMock = new Mock<ICognitoService>();
        _authenticateUseCase = new AuthenticateAdminUseCase(_cognitoServiceMock.Object);
        _controller = new AdminController(_authenticateUseCase);
    }

    [Fact]
    public async Task PostLogin_WithValidCredentials_ShouldReturnOkWithTokens()
    {
        // Arrange
        var inputModel = new AuthenticateAdminInputModel
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
            .Setup(x => x.AuthenticateAsync(inputModel.Username, inputModel.Password))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.Login(inputModel);

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
        var inputModel = new AuthenticateAdminInputModel
        {
            Username = "admin@example.com",
            Password = "WrongPassword"
        };

        _cognitoServiceMock
            .Setup(x => x.AuthenticateAsync(inputModel.Username, inputModel.Password))
            .ThrowsAsync(new UnauthorizedAccessException("Credenciais inválidas"));

        // Act
        var result = await _controller.Login(inputModel);

        // Assert
        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.NotNull(unauthorizedResult.Value);
    }

    [Fact]
    public async Task PostLogin_ShouldCallUseCase()
    {
        // Arrange
        var inputModel = new AuthenticateAdminInputModel
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
            .Setup(x => x.AuthenticateAsync(inputModel.Username, inputModel.Password))
            .ReturnsAsync(expectedResult);

        // Act
        await _controller.Login(inputModel);

        // Assert
        _cognitoServiceMock.Verify(
            x => x.AuthenticateAsync(inputModel.Username, inputModel.Password), 
            Times.Once);
    }

    [Fact]
    public async Task PostLogin_WhenGenericExceptionOccurs_ShouldReturnStatusCode500()
    {
        // Arrange
        var inputModel = new AuthenticateAdminInputModel
        {
            Username = "admin@example.com",
            Password = "SecurePassword123!"
        };

        _cognitoServiceMock
            .Setup(x => x.AuthenticateAsync(inputModel.Username, inputModel.Password))
            .ThrowsAsync(new Exception("Service error"));

        // Act
        var result = await _controller.Login(inputModel);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }
}

