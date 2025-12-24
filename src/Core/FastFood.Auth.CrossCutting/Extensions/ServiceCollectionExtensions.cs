using Microsoft.Extensions.DependencyInjection;
using Amazon.Lambda.AspNetCoreServer.Hosting;

namespace FastFood.Auth.CrossCutting.Extensions;

/// <summary>
/// Extension methods para configuração de serviços comuns em Lambdas.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adiciona serviços comuns para Lambdas (AWS Hosting, Controllers, Swagger).
    /// </summary>
    public static IServiceCollection AddLambdaCommonServices(this IServiceCollection services)
    {
        services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        return services;
    }
}

