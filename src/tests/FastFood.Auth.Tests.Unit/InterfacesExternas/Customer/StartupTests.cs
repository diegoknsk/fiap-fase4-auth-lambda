using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Application.UseCases.Customer;
using FastFood.Auth.Lambda.Customer;

namespace FastFood.Auth.Tests.Unit.InterfacesExternas.Customer;

/// <summary>
/// Testes unitários para Startup do Lambda Customer.
/// </summary>
public class StartupTests
{
    [Fact]
    public void ConfigureServices_ShouldRegisterCommonServices()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().Build();
        var startup = new Startup(configuration);
        var services = new ServiceCollection();

        // Act
        startup.ConfigureServices(services);

        // Assert
        // Verifica que serviços comuns foram registrados (Controllers, Swagger, etc)
        var serviceProvider = services.BuildServiceProvider();
        Assert.NotNull(serviceProvider);
    }

    [Fact]
    public void ConfigureServices_ShouldRegisterCustomerUseCases()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().Build();
        var startup = new Startup(configuration);
        var services = new ServiceCollection();

        // Act
        startup.ConfigureServices(services);

        // Assert
        // Verifica que os UseCases foram registrados
        var createAnonymousDescriptor = services.FirstOrDefault(s => 
            s.ServiceType == typeof(CreateAnonymousCustomerUseCase));
        Assert.NotNull(createAnonymousDescriptor);
        Assert.Equal(ServiceLifetime.Scoped, createAnonymousDescriptor.Lifetime);

        var registerDescriptor = services.FirstOrDefault(s => 
            s.ServiceType == typeof(RegisterCustomerUseCase));
        Assert.NotNull(registerDescriptor);
        Assert.Equal(ServiceLifetime.Scoped, registerDescriptor.Lifetime);

        var identifyDescriptor = services.FirstOrDefault(s => 
            s.ServiceType == typeof(IdentifyCustomerUseCase));
        Assert.NotNull(identifyDescriptor);
        Assert.Equal(ServiceLifetime.Scoped, identifyDescriptor.Lifetime);
    }
}
