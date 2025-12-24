using Amazon.Lambda.AspNetCoreServer;
using Amazon.Lambda.AspNetCoreServer.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Infra.Services;
using FastFood.Auth.Application.UseCases.Admin;
using FastFood.Auth.CrossCutting.Extensions;

namespace FastFood.Auth.Lambda.Admin;

public class LambdaEntryPoint : APIGatewayHttpApiV2ProxyFunction
{
    protected override void Init(IWebHostBuilder builder)
    {
        builder
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseStartup<Startup>();
    }
}

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        // Configuration is available via dependency injection when needed
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddLambdaCommonServices();
        ConfigureAdminSpecificServices(services);
    }

    private static void ConfigureAdminSpecificServices(IServiceCollection services)
    {
        services.AddScoped<ICognitoService, CognitoService>();
        services.AddScoped<AuthenticateAdminUseCase>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwaggerInDevelopment(env);
        app.UseGlobalExceptionHandler(env);
        app.UseDefaultRouting();
    }
}

