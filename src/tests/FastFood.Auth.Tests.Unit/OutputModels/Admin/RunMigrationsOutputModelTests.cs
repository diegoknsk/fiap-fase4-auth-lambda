using FastFood.Auth.Application.OutputModels.Admin;

namespace FastFood.Auth.Tests.Unit.OutputModels.Admin;

/// <summary>
/// Testes unitários para RunMigrationsOutputModel.
/// </summary>
public class RunMigrationsOutputModelTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var model = new RunMigrationsOutputModel();

        // Assert
        Assert.False(model.Success);
        Assert.Equal(string.Empty, model.Message);
        Assert.Equal(0, model.PendingMigrationsCount);
        Assert.Equal(0, model.AppliedMigrationsCount);
        Assert.NotNull(model.PendingMigrations);
        Assert.NotNull(model.AppliedMigrations);
        Assert.Empty(model.PendingMigrations);
        Assert.Empty(model.AppliedMigrations);
        Assert.Null(model.Error);
    }

    [Fact]
    public void Properties_ShouldSetAndGetValues()
    {
        // Arrange
        var model = new RunMigrationsOutputModel
        {
            Success = true,
            Message = "Test message",
            PendingMigrationsCount = 2,
            PendingMigrations = new List<string> { "Migration1", "Migration2" },
            AppliedMigrationsCount = 3,
            AppliedMigrations = new List<string> { "Migration1", "Migration2", "Migration3" },
            Error = "Test error"
        };

        // Assert
        Assert.True(model.Success);
        Assert.Equal("Test message", model.Message);
        Assert.Equal(2, model.PendingMigrationsCount);
        Assert.Equal(3, model.AppliedMigrationsCount);
        Assert.Equal(2, model.PendingMigrations.Count);
        Assert.Equal(3, model.AppliedMigrations.Count);
        Assert.Equal("Test error", model.Error);
    }

    [Fact]
    public void PendingMigrations_ShouldBeMutable()
    {
        // Arrange
        var model = new RunMigrationsOutputModel();

        // Act
        model.PendingMigrations.Add("Migration1");
        model.PendingMigrations.Add("Migration2");

        // Assert
        Assert.Equal(2, model.PendingMigrations.Count);
        Assert.Contains("Migration1", model.PendingMigrations);
        Assert.Contains("Migration2", model.PendingMigrations);
    }

    [Fact]
    public void AppliedMigrations_ShouldBeMutable()
    {
        // Arrange
        var model = new RunMigrationsOutputModel();

        // Act
        model.AppliedMigrations.Add("Migration1");
        model.AppliedMigrations.Add("Migration2");
        model.AppliedMigrations.Add("Migration3");

        // Assert
        Assert.Equal(3, model.AppliedMigrations.Count);
        Assert.Contains("Migration1", model.AppliedMigrations);
        Assert.Contains("Migration2", model.AppliedMigrations);
        Assert.Contains("Migration3", model.AppliedMigrations);
    }

    [Fact]
    public void Error_ShouldBeNullable()
    {
        // Arrange
        var model = new RunMigrationsOutputModel();

        // Act & Assert
        Assert.Null(model.Error);

        model.Error = "Error message";
        Assert.Equal("Error message", model.Error);

        model.Error = null;
        Assert.Null(model.Error);
    }
}

