using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FastFood.Auth.Infra.Persistence;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Infra.Services;
using Moq;
using FastFood.Auth.Lambda;

namespace FastFood.Auth.Tests.Bdd.Support;

/// <summary>
/// Fixture para criar uma instância da aplicação para testes BDD.
/// </summary>
public class WebApplicationFactoryFixture : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
        {
            // Adicionar configurações necessárias para os testes
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "JwtSettings:Secret", "TestSecretKeyForJWTTokenGenerationInBDDTests12345678901234567890" },
                { "JwtSettings:Issuer", "FastFood.Auth.Tests" },
                { "JwtSettings:Audience", "FastFood.Auth.Tests" },
                { "JwtSettings:ExpirationHours", "24" },
                { "ConnectionStrings:DefaultConnection", "Host=localhost;Port=5432;Database=testdb" }
            });
        });

        builder.ConfigureServices(services =>
        {
            // Remover o DbContext real
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AuthDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Adicionar DbContext em memória para testes
            // Usar nome fixo para garantir que o mesmo banco seja usado
            var databaseName = "TestDb_BDD";
            services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseInMemoryDatabase(databaseName);
            });

            // Remover serviços reais e adicionar mocks
            var cognitoServiceDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(ICognitoService));
            if (cognitoServiceDescriptor != null)
            {
                services.Remove(cognitoServiceDescriptor);
            }

            // Criar mock do CognitoService - usar AddSingleton para manter a mesma instância
            var cognitoServiceMock = new Mock<ICognitoService>();
            // Configurar comportamento padrão (pode ser sobrescrito nos steps)
            cognitoServiceMock.Setup(x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new FastFood.Auth.Application.Ports.AuthenticateAdminResult
                {
                    AccessToken = "default-access-token",
                    IdToken = "default-id-token",
                    ExpiresIn = 3600,
                    TokenType = "Bearer"
                });
            
            // Adicionar o mock como singleton para que possa ser acessado e reconfigurado
            services.AddSingleton(cognitoServiceMock);
            services.AddSingleton<ICognitoService>(sp => sp.GetRequiredService<Mock<ICognitoService>>().Object);
        });

        builder.UseEnvironment("Testing");
    }
}

