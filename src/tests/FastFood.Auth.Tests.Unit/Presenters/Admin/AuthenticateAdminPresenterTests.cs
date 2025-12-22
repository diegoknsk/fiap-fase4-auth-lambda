using FastFood.Auth.Application.OutputModels.Admin;
using FastFood.Auth.Application.Presenters.Admin;

namespace FastFood.Auth.Tests.Unit.Presenters.Admin;

/// <summary>
/// Testes unitários para AuthenticateAdminPresenter.
/// </summary>
public class AuthenticateAdminPresenterTests
{

    [Fact]
    public void Present_ShouldReturnSameOutputModel()
    {
        // Arrange
        var outputModel = new AuthenticateAdminOutputModel
        {
            AccessToken = "access-token-123",
            IdToken = "id-token-456",
            ExpiresIn = 3600,
            TokenType = "Bearer"
        };

        // Act
        var result = AuthenticateAdminPresenter.Present(outputModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(outputModel.AccessToken, result.AccessToken);
        Assert.Equal(outputModel.IdToken, result.IdToken);
        Assert.Equal(outputModel.ExpiresIn, result.ExpiresIn);
        Assert.Equal(outputModel.TokenType, result.TokenType);
    }

    [Fact]
    public void Present_ShouldReturnSameInstance()
    {
        // Arrange
        var outputModel = new AuthenticateAdminOutputModel
        {
            AccessToken = "access-token-123",
            IdToken = "id-token-456",
            ExpiresIn = 3600,
            TokenType = "Bearer"
        };

        // Act
        var result = AuthenticateAdminPresenter.Present(outputModel);

        // Assert
        Assert.Same(outputModel, result);
    }

    [Fact]
    public void Present_WithNullTokens_ShouldReturnOutputModel()
    {
        // Arrange
        var outputModel = new AuthenticateAdminOutputModel
        {
            AccessToken = null!,
            IdToken = null!,
            ExpiresIn = 3600,
            TokenType = "Bearer"
        };

        // Act
        var result = AuthenticateAdminPresenter.Present(outputModel);

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.AccessToken);
        Assert.Null(result.IdToken);
        Assert.Equal(outputModel.ExpiresIn, result.ExpiresIn);
        Assert.Equal(outputModel.TokenType, result.TokenType);
    }

    [Fact]
    public void Present_WithZeroExpiresIn_ShouldReturnOutputModel()
    {
        // Arrange
        var outputModel = new AuthenticateAdminOutputModel
        {
            AccessToken = "access-token-123",
            IdToken = "id-token-456",
            ExpiresIn = 0,
            TokenType = "Bearer"
        };

        // Act
        var result = AuthenticateAdminPresenter.Present(outputModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0, result.ExpiresIn);
    }
}



