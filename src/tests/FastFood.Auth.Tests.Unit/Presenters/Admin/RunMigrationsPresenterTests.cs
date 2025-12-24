using FastFood.Auth.Application.OutputModels.Admin;
using FastFood.Auth.Application.Presenters.Admin;

namespace FastFood.Auth.Tests.Unit.Presenters.Admin;

/// <summary>
/// Testes unitários para RunMigrationsPresenter.
/// </summary>
public class RunMigrationsPresenterTests
{
    [Fact]
    public void Present_WithValidOutputModel_ShouldReturnSameModel()
    {
        // Arrange
        var outputModel = new RunMigrationsOutputModel
        {
            Success = true,
            Message = "Test message",
            PendingMigrationsCount = 2,
            PendingMigrations = new List<string> { "Migration1", "Migration2" },
            AppliedMigrationsCount = 3,
            AppliedMigrations = new List<string> { "Migration1", "Migration2", "Migration3" }
        };

        // Act
        var result = RunMigrationsPresenter.Present(outputModel);

        // Assert
        Assert.NotNull(result);
        Assert.Same(outputModel, result);
        Assert.True(result.Success);
        Assert.Equal("Test message", result.Message);
        Assert.Equal(2, result.PendingMigrationsCount);
        Assert.Equal(3, result.AppliedMigrationsCount);
    }

    [Fact]
    public void Present_WithEmptyMigrations_ShouldReturnModel()
    {
        // Arrange
        var outputModel = new RunMigrationsOutputModel
        {
            Success = true,
            Message = "No migrations",
            PendingMigrationsCount = 0,
            PendingMigrations = new List<string>(),
            AppliedMigrationsCount = 0,
            AppliedMigrations = new List<string>()
        };

        // Act
        var result = RunMigrationsPresenter.Present(outputModel);

        // Assert
        Assert.NotNull(result);
        Assert.Same(outputModel, result);
        Assert.Equal(0, result.PendingMigrationsCount);
        Assert.Equal(0, result.AppliedMigrationsCount);
        Assert.Empty(result.PendingMigrations);
        Assert.Empty(result.AppliedMigrations);
    }

    [Fact]
    public void Present_WithError_ShouldReturnModelWithError()
    {
        // Arrange
        var outputModel = new RunMigrationsOutputModel
        {
            Success = false,
            Message = "Error occurred",
            Error = "Database connection failed",
            PendingMigrationsCount = 0,
            PendingMigrations = new List<string>(),
            AppliedMigrationsCount = 0,
            AppliedMigrations = new List<string>()
        };

        // Act
        var result = RunMigrationsPresenter.Present(outputModel);

        // Assert
        Assert.NotNull(result);
        Assert.Same(outputModel, result);
        Assert.False(result.Success);
        Assert.Equal("Error occurred", result.Message);
        Assert.Equal("Database connection failed", result.Error);
    }
}

