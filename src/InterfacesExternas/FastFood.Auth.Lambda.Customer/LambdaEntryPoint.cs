using Amazon.Lambda.AspNetCoreServer;
using Amazon.Lambda.AspNetCoreServer.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FastFood.Auth.Infra.Persistence;
using Microsoft.EntityFrameworkCore;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Infra.Persistence.Repositories;
using FastFood.Auth.Infra.Persistence.Services;
using FastFood.Auth.Infra.Services;
using FastFood.Auth.Application.UseCases.Customer;

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
        RegisterCommonServices(services);
        RegisterDatabaseContext(services);
        RegisterCustomerSpecificServices(services);
    }

    private static void RegisterCommonServices(IServiceCollection services)
    {
        services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
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
        SetupSwagger(app, env);
        SetupExceptionHandling(app, env);
        SetupRouting(app);
    }

    private static void SetupSwagger(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }

    private static void SetupExceptionHandling(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseExceptionHandler(appBuilder =>
        {
            appBuilder.Run(async context =>
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                var logger = context.RequestServices.GetRequiredService<ILogger<Startup>>();
                var exceptionHandlerFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
                var exception = exceptionHandlerFeature?.Error;
                if (exception != null)
                {
                    logger.LogError(exception, "Erro não tratado na aplicação");
                    var isDevelopment = env.IsDevelopment();
                    var errorDetails = new { message = "Erro interno do servidor", error = isDevelopment ? exception.Message : null };
                    await context.Response.WriteAsJsonAsync(errorDetails);
                }
            });
        });
    }

    private static void SetupRouting(IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}

