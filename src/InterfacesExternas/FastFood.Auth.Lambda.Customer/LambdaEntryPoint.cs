using Amazon.Lambda.AspNetCoreServer;
using Amazon.Lambda.AspNetCoreServer.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FastFood.Auth.Infra.Persistence;
using Microsoft.EntityFrameworkCore;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Infra.Persistence.Repositories;
using FastFood.Auth.Infra.Persistence.Services;
using FastFood.Auth.Infra.Services;
using FastFood.Auth.Application.UseCases.Customer;
using FastFood.Auth.CrossCutting.Extensions;

namespace FastFood.Auth.Lambda.Customer;

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
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddLambdaCommonServices();
        RegisterDatabaseContext(services);
        RegisterCustomerSpecificServices(services);
    }

    private void RegisterDatabaseContext(IServiceCollection services)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        if (!string.IsNullOrEmpty(connectionString))
        {
            services.AddDbContext<AuthDbContext>(options =>
                options.UseNpgsql(connectionString));
        }
    }

    private static void RegisterCustomerSpecificServices(IServiceCollection services)
    {
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<CreateAnonymousCustomerUseCase>();
        services.AddScoped<RegisterCustomerUseCase>();
        services.AddScoped<IdentifyCustomerUseCase>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwaggerInDevelopment(env);
        app.UseGlobalExceptionHandler(env);
        app.UseDefaultRouting();
    }
}

