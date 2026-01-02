using System.Reflection;

namespace FastFood.Auth.Tests.Unit.InterfacesExternas.Migrator;

/// <summary>
/// Testes unitários para Program do Migrator.
/// </summary>
public class ProgramTests
{
    private static Type GetProgramType()
    {
        var assembly = typeof(FastFood.Auth.Infra.Persistence.AuthDbContext).Assembly;
        // Buscar o assembly do Migrator através de uma referência conhecida
        var migratorAssembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a => a.GetName().Name == "FastFood.Auth.Migrator");
        
        if (migratorAssembly == null)
        {
            // Se não encontrou, tentar carregar
            var migratorPath = Path.Combine(AppContext.BaseDirectory, "FastFood.Auth.Migrator.dll");
            if (File.Exists(migratorPath))
            {
                migratorAssembly = Assembly.LoadFrom(migratorPath);
            }
        }
        
        Assert.NotNull(migratorAssembly);
        var programType = migratorAssembly.GetType("FastFood.Auth.Migrator.Program");
        Assert.NotNull(programType);
        return programType;
    }

    [Fact]
    public void ExtractHostFromConnectionString_WithValidHost_ShouldReturnHost()
    {
        // Arrange
        var connectionString = "Host=localhost;Database=testdb;Username=test;Password=test";
        var programType = GetProgramType();
        var method = programType.GetMethod("ExtractHostFromConnectionString",
            BindingFlags.NonPublic | BindingFlags.Static);
        
        Assert.NotNull(method);

        // Act
        var result = method.Invoke(null, new object[] { connectionString }) as string;

        // Assert
        Assert.Equal("localhost", result);
    }

    [Fact]
    public void ExtractHostFromConnectionString_WithHostInDifferentCase_ShouldReturnHost()
    {
        // Arrange
        var connectionString = "HOST=example.com;Database=testdb";
        var programType = GetProgramType();
        var method = programType.GetMethod("ExtractHostFromConnectionString", 
            BindingFlags.NonPublic | BindingFlags.Static);
        
        Assert.NotNull(method);

        // Act
        var result = method.Invoke(null, new object[] { connectionString }) as string;

        // Assert
        Assert.Equal("example.com", result);
    }

    [Fact]
    public void ExtractHostFromConnectionString_WithoutHost_ShouldReturnNA()
    {
        // Arrange
        var connectionString = "Database=testdb;Username=test;Password=test";
        var programType = GetProgramType();
        var method = programType.GetMethod("ExtractHostFromConnectionString", 
            BindingFlags.NonPublic | BindingFlags.Static);
        
        Assert.NotNull(method);

        // Act
        var result = method.Invoke(null, new object[] { connectionString }) as string;

        // Assert
        Assert.Equal("N/A", result);
    }

    [Fact]
    public void ExtractHostFromConnectionString_WithEmptyString_ShouldReturnNA()
    {
        // Arrange
        var connectionString = "";
        var programType = GetProgramType();
        var method = programType.GetMethod("ExtractHostFromConnectionString", 
            BindingFlags.NonPublic | BindingFlags.Static);
        
        Assert.NotNull(method);

        // Act
        var result = method.Invoke(null, new object[] { connectionString }) as string;

        // Assert
        Assert.Equal("N/A", result);
    }

    [Fact]
    public void ExtractHostFromConnectionString_WithHostAtEnd_ShouldReturnHost()
    {
        // Arrange
        var connectionString = "Database=testdb;Host=myhost.example.com";
        var programType = GetProgramType();
        var method = programType.GetMethod("ExtractHostFromConnectionString", 
            BindingFlags.NonPublic | BindingFlags.Static);
        
        Assert.NotNull(method);

        // Act
        var result = method.Invoke(null, new object[] { connectionString }) as string;

        // Assert
        Assert.Equal("myhost.example.com", result);
    }

    [Fact]
    public void ExtractHostFromConnectionString_WithHostWithPort_ShouldReturnHostWithPort()
    {
        // Arrange
        var connectionString = "Host=localhost:5432;Database=testdb";
        var programType = GetProgramType();
        var method = programType.GetMethod("ExtractHostFromConnectionString", 
            BindingFlags.NonPublic | BindingFlags.Static);
        
        Assert.NotNull(method);

        // Act
        var result = method.Invoke(null, new object[] { connectionString }) as string;

        // Assert
        Assert.Equal("localhost:5432", result);
    }
}
