using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Infra.Services;
using Moq;
using FastFood.Auth.Lambda.Admin;

namespace FastFood.Auth.Tests.Bdd.Support;

/// <summary>
/// Fixture para criar uma instância da aplicação Admin para testes BDD.
/// </summary>
public class WebApplicationFactoryFixtureAdmin : WebApplicationFactory<LambdaEntryPoint>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Configurar variável de ambiente para JWT Secret (obrigatório)
        // NOTA: Secret hardcoded é aceitável aqui pois é apenas para testes BDD em ambiente isolado
        // Em produção, o secret vem de variáveis de ambiente configuradas via Terraform
        Environment.SetEnvironmentVariable("JwtSettings__Secret", "TestSecretKeyForJWTTokenGenerationInBDDTests12345678901234567890");

        builder.ConfigureAppConfiguration(config =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "JwtSettings:Issuer", "FastFood.Auth.Tests" },
                { "JwtSettings:Audience", "FastFood.Auth.Tests" },
                { "JwtSettings:ExpirationHours", "24" }
            });
        });

        builder.ConfigureServices(services =>
        {
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

