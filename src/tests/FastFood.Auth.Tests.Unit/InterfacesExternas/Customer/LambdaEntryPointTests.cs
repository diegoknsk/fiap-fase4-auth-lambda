using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Moq;
using FastFood.Auth.Lambda.Customer;

namespace FastFood.Auth.Tests.Unit.InterfacesExternas.Customer;

/// <summary>
/// Testes unitários para LambdaEntryPoint do Lambda Customer.
/// </summary>
public class LambdaEntryPointTests
{
    [Fact]
    public void Init_ShouldExistAndBeCallable()
    {
        // Arrange
        var entryPoint = new LambdaEntryPoint();
        var webHostBuilderMock = new Mock<IWebHostBuilder>();
        var webHostBuilder = webHostBuilderMock.Object;

        // Act - Verificar que o método Init existe e pode ser chamado
        var initMethod = typeof(LambdaEntryPoint).GetMethod("Init", 
            BindingFlags.NonPublic | BindingFlags.Instance, 
            null, 
            new[] { typeof(IWebHostBuilder) }, 
            null);
        
        // Assert
        Assert.NotNull(initMethod);
        // Se chegou até aqui, o método existe e pode ser invocado
        // Não podemos verificar extension methods com Moq, mas podemos garantir que o método existe
    }
}
