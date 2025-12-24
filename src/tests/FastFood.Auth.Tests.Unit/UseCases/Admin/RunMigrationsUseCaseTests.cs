using Moq;
using Microsoft.Extensions.Logging;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Application.UseCases.Admin;
using FastFood.Auth.Application.OutputModels.Admin;

namespace FastFood.Auth.Tests.Unit.UseCases.Admin;

/// <summary>
/// Testes unitários para RunMigrationsUseCase.
/// </summary>
public class RunMigrationsUseCaseTests
{
    private readonly Mock<IMigrationService> _migrationServiceMock;
    private readonly Mock<ILogger<RunMigrationsUseCase>> _loggerMock;
    private readonly RunMigrationsUseCase _useCase;

    public RunMigrationsUseCaseTests()
    {
        _migrationServiceMock = new Mock<IMigrationService>();
        _loggerMock = new Mock<ILogger<RunMigrationsUseCase>>();
        _useCase = new RunMigrationsUseCase(_migrationServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenPendingMigrationsExist_ShouldApplyMigrations()
    {
        // Arrange
        var pendingMigrations = new[] { "20240101000000_InitialMigration", "20240102000000_AddCustomerTable" };
        var appliedMigrations = new[] { "20240101000000_InitialMigration", "20240102000000_AddCustomerTable", "20240103000000_AddIndexes" };

        _migrationServiceMock
            .Setup(x => x.GetPendingMigrationsAsync())
            .ReturnsAsync(pendingMigrations);

        _migrationServiceMock
            .Setup(x => x.GetAppliedMigrationsAsync())
            .ReturnsAsync(appliedMigrations);

        // Act
        var result = await _useCase.ExecuteAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Equal(2, result.PendingMigrationsCount);
        Assert.Equal(3, result.AppliedMigrationsCount);
        Assert.Equal(2, result.PendingMigrations.Count);
        Assert.Equal(3, result.AppliedMigrations.Count);
        Assert.Contains("aplicada(s) com sucesso", result.Message);

        _migrationServiceMock.Verify(x => x.GetPendingMigrationsAsync(), Times.Once);
        _migrationServiceMock.Verify(x => x.ApplyMigrationsAsync(), Times.Once);
        _migrationServiceMock.Verify(x => x.GetAppliedMigrationsAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenNoPendingMigrations_ShouldNotApplyMigrations()
    {
        // Arrange
        var pendingMigrations = Array.Empty<string>();
        var appliedMigrations = new[] { "20240101000000_InitialMigration", "20240102000000_AddCustomerTable" };

        _migrationServiceMock
            .Setup(x => x.GetPendingMigrationsAsync())
            .ReturnsAsync(pendingMigrations);

        _migrationServiceMock
            .Setup(x => x.GetAppliedMigrationsAsync())
            .ReturnsAsync(appliedMigrations);

        // Act
        var result = await _useCase.ExecuteAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Equal(0, result.PendingMigrationsCount);
        Assert.Equal(2, result.AppliedMigrationsCount);
        Assert.Empty(result.PendingMigrations);
        Assert.Equal(2, result.AppliedMigrations.Count);
        Assert.Contains("Nenhuma migration pendente", result.Message);

        _migrationServiceMock.Verify(x => x.GetPendingMigrationsAsync(), Times.Once);
        _migrationServiceMock.Verify(x => x.ApplyMigrationsAsync(), Times.Never);
        _migrationServiceMock.Verify(x => x.GetAppliedMigrationsAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenSinglePendingMigration_ShouldApplyAndReturnCorrectMessage()
    {
        // Arrange
        var pendingMigrations = new[] { "20240101000000_InitialMigration" };
        var appliedMigrations = new[] { "20240101000000_InitialMigration" };

        _migrationServiceMock
            .Setup(x => x.GetPendingMigrationsAsync())
            .ReturnsAsync(pendingMigrations);

        _migrationServiceMock
            .Setup(x => x.GetAppliedMigrationsAsync())
            .ReturnsAsync(appliedMigrations);

        // Act
        var result = await _useCase.ExecuteAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Equal(1, result.PendingMigrationsCount);
        Assert.Equal(1, result.AppliedMigrationsCount);
        Assert.Single(result.PendingMigrations);
        Assert.Single(result.AppliedMigrations);
        Assert.Contains("1 migration(s) aplicada(s)", result.Message);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldLogInformationMessages()
    {
        // Arrange
        var pendingMigrations = new[] { "20240101000000_InitialMigration" };
        var appliedMigrations = new[] { "20240101000000_InitialMigration" };

        _migrationServiceMock
            .Setup(x => x.GetPendingMigrationsAsync())
            .ReturnsAsync(pendingMigrations);

        _migrationServiceMock
            .Setup(x => x.GetAppliedMigrationsAsync())
            .ReturnsAsync(appliedMigrations);

        // Act
        await _useCase.ExecuteAsync();

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Verificando migrations pendentes")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Aplicando migrations")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenNoPendingMigrations_ShouldLogNoPendingMessage()
    {
        // Arrange
        var pendingMigrations = Array.Empty<string>();
        var appliedMigrations = new[] { "20240101000000_InitialMigration" };

        _migrationServiceMock
            .Setup(x => x.GetPendingMigrationsAsync())
            .ReturnsAsync(pendingMigrations);

        _migrationServiceMock
            .Setup(x => x.GetAppliedMigrationsAsync())
            .ReturnsAsync(appliedMigrations);

        // Act
        await _useCase.ExecuteAsync();

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Nenhuma migration pendente")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnOutputModelWithAllProperties()
    {
        // Arrange
        var pendingMigrations = new[] { "20240101000000_InitialMigration", "20240102000000_AddCustomerTable" };
        var appliedMigrations = new[] { "20240101000000_InitialMigration", "20240102000000_AddCustomerTable" };

        _migrationServiceMock
            .Setup(x => x.GetPendingMigrationsAsync())
            .ReturnsAsync(pendingMigrations);

        _migrationServiceMock
            .Setup(x => x.GetAppliedMigrationsAsync())
            .ReturnsAsync(appliedMigrations);

        // Act
        var result = await _useCase.ExecuteAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotNull(result.Message);
        Assert.NotEmpty(result.Message);
        Assert.Equal(2, result.PendingMigrations.Count);
        Assert.Equal(2, result.AppliedMigrations.Count);
        Assert.Equal("20240101000000_InitialMigration", result.PendingMigrations[0]);
        Assert.Equal("20240102000000_AddCustomerTable", result.PendingMigrations[1]);
    }

    [Fact]
    public async Task ExecuteAsync_WhenPendingMigrationsExist_ShouldLogSuccessMessage()
    {
        // Arrange
        var pendingMigrations = new[] { "20240101000000_InitialMigration" };
        var appliedMigrations = new[] { "20240101000000_InitialMigration" };

        _migrationServiceMock
            .Setup(x => x.GetPendingMigrationsAsync())
            .ReturnsAsync(pendingMigrations);

        _migrationServiceMock
            .Setup(x => x.GetAppliedMigrationsAsync())
            .ReturnsAsync(appliedMigrations);

        // Act
        await _useCase.ExecuteAsync();

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Encontradas")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Migrations aplicadas com sucesso")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}

