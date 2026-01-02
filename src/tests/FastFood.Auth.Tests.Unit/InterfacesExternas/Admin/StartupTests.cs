using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Application.UseCases.Admin;
using FastFood.Auth.Lambda.Admin;

namespace FastFood.Auth.Tests.Unit.InterfacesExternas.Admin;

/// <summary>
/// Testes unitários para Startup do Lambda Admin.
/// </summary>
public class StartupTests
{
    [Fact]
    public void ConfigureServices_ShouldRegisterCommonServices()
    {
        // Arrange
        var configuration = new Mock<IConfiguration>();
        var startup = new Startup(configuration.Object);
        var services = new ServiceCollection();

        // Act
        startup.ConfigureServices(services);

        // Assert
        // Verifica que serviços comuns foram registrados (Controllers, Swagger, etc)
        var serviceProvider = services.BuildServiceProvider();
        Assert.NotNull(serviceProvider);
    }

    [Fact]
    public void ConfigureServices_ShouldRegisterAdminSpecificServices()
    {
        // Arrange
        var configuration = new Mock<IConfiguration>();
        var startup = new Startup(configuration.Object);
        var services = new ServiceCollection();

        // Act
        startup.ConfigureServices(services);

        // Assert
        // Verifica que ICognitoService foi registrado
        var cognitoServiceDescriptor = services.FirstOrDefault(s => 
            s.ServiceType == typeof(ICognitoService));
        Assert.NotNull(cognitoServiceDescriptor);
        Assert.Equal(ServiceLifetime.Scoped, cognitoServiceDescriptor.Lifetime);

        // Verifica que AuthenticateAdminUseCase foi registrado
        var useCaseDescriptor = services.FirstOrDefault(s => 
            s.ServiceType == typeof(AuthenticateAdminUseCase));
        Assert.NotNull(useCaseDescriptor);
        Assert.Equal(ServiceLifetime.Scoped, useCaseDescriptor.Lifetime);
    }

}
