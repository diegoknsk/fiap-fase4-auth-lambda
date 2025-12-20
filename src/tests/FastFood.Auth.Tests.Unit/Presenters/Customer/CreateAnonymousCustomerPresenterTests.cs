using FastFood.Auth.Application.OutputModels.Customer;
using FastFood.Auth.Application.Presenters.Customer;

namespace FastFood.Auth.Tests.Unit.Presenters.Customer;

/// <summary>
/// Testes unitários para CreateAnonymousCustomerPresenter.
/// </summary>
public class CreateAnonymousCustomerPresenterTests
{
    private readonly CreateAnonymousCustomerPresenter _presenter;

    public CreateAnonymousCustomerPresenterTests()
    {
        _presenter = new CreateAnonymousCustomerPresenter();
    }

    [Fact]
    public void Present_ShouldReturnSameOutputModel()
    {
        // Arrange
        var outputModel = new CreateAnonymousCustomerOutputModel
        {
            Token = "test-token",
            CustomerId = Guid.NewGuid(),
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        };

        // Act
        var result = _presenter.Present(outputModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(outputModel.Token, result.Token);
        Assert.Equal(outputModel.CustomerId, result.CustomerId);
        Assert.Equal(outputModel.ExpiresAt, result.ExpiresAt);
    }

    [Fact]
    public void Present_ShouldReturnSameInstance()
    {
        // Arrange
        var outputModel = new CreateAnonymousCustomerOutputModel
        {
            Token = "test-token",
            CustomerId = Guid.NewGuid(),
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        };

        // Act
        var result = _presenter.Present(outputModel);

        // Assert
        Assert.Same(outputModel, result);
    }

    [Fact]
    public void Present_WithNullToken_ShouldReturnOutputModel()
    {
        // Arrange
        var outputModel = new CreateAnonymousCustomerOutputModel
        {
            Token = null!,
            CustomerId = Guid.NewGuid(),
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        };

        // Act
        var result = _presenter.Present(outputModel);

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.Token);
        Assert.Equal(outputModel.CustomerId, result.CustomerId);
    }
}


