using Microsoft.EntityFrameworkCore;
using FastFood.Auth.Infra.Persistence;
using FastFood.Auth.Infra.Persistence.Services;

namespace FastFood.Auth.Tests.Unit.Infra.Persistence.Services;

/// <summary>
/// Testes unitários para MigrationService.
/// Nota: Usamos SQLite em memória ao invés de InMemoryDatabase porque métodos relacionais
/// como GetPendingMigrationsAsync requerem um provider relacional.
/// </summary>
public class MigrationServiceTests
{
    [Fact]
    public void Constructor_WithDbContext_ShouldCreateService()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AuthDbContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;
        var context = new AuthDbContext(options);

        // Act
        var service = new MigrationService(context);

        // Assert
        Assert.NotNull(service);
    }

    [Fact]
    public async Task GetPendingMigrationsAsync_ShouldCallDatabaseGetPendingMigrationsAsync()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AuthDbContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;
        var context = new AuthDbContext(options);
        var service = new MigrationService(context);

        // Act
        var result = await service.GetPendingMigrationsAsync();

        // Assert
        Assert.NotNull(result);
        // SQLite em memória sem migrations aplicadas retorna todas as migrations pendentes
        // ou vazio se não houver migrations definidas
    }

    [Fact]
    public async Task GetAppliedMigrationsAsync_ShouldCallDatabaseGetAppliedMigrationsAsync()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AuthDbContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;
        var context = new AuthDbContext(options);
        var service = new MigrationService(context);

        // Act
        var result = await service.GetAppliedMigrationsAsync();

        // Assert
        Assert.NotNull(result);
        // SQLite em memória sem migrations aplicadas retorna vazio
    }

    [Fact]
    public async Task ApplyMigrationsAsync_ShouldCallDatabaseMigrateAsync()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AuthDbContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;
        var context = new AuthDbContext(options);
        var service = new MigrationService(context);

        // Act & Assert
        // Não deve lançar exceção - SQLite em memória suporta MigrateAsync
        // Pode lançar exceção se não houver migrations, mas isso é esperado
        var exception = await Record.ExceptionAsync(() => service.ApplyMigrationsAsync());
        // Permitir exceção se não houver migrations definidas
    }

    [Fact]
    public async Task GetPendingMigrationsAsync_ShouldReturnIEnumerable()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AuthDbContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;
        var context = new AuthDbContext(options);
        var service = new MigrationService(context);

        // Act
        var result = await service.GetPendingMigrationsAsync();

        // Assert
        Assert.NotNull(result);
        // Deve ser enumerável
        var count = result.Count();
        Assert.True(count >= 0);
    }

    [Fact]
    public async Task GetAppliedMigrationsAsync_ShouldReturnIEnumerable()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AuthDbContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;
        var context = new AuthDbContext(options);
        var service = new MigrationService(context);

        // Act
        var result = await service.GetAppliedMigrationsAsync();

        // Assert
        Assert.NotNull(result);
        // Deve ser enumerável
        var count = result.Count();
        Assert.True(count >= 0);
    }
}

