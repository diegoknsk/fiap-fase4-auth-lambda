using Amazon.Lambda.AspNetCoreServer;
using Amazon.Lambda.AspNetCoreServer.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Infra.Services;
using FastFood.Auth.Application.UseCases.Admin;

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
        ConfigureCommonServices(services);
        ConfigureAdminSpecificServices(services);
    }

    private static void ConfigureCommonServices(IServiceCollection services)
    {
        services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    private static void ConfigureAdminSpecificServices(IServiceCollection services)
    {
        services.AddScoped<ICognitoService, CognitoService>();
        services.AddScoped<AuthenticateAdminUseCase>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        ConfigureSwagger(app, env);
        ConfigureExceptionHandling(app, env);
        ConfigureRouting(app);
    }

    private static void ConfigureSwagger(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }

    private static void ConfigureExceptionHandling(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseExceptionHandler(appBuilder =>
        {
            appBuilder.Run(async context =>
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                var logger = context.RequestServices.GetRequiredService<ILogger<Startup>>();
                var exception = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;
                if (exception != null)
                {
                    logger.LogError(exception, "Erro não tratado na aplicação");
                    var errorResponse = new { message = "Erro interno do servidor", error = env.IsDevelopment() ? exception.Message : null };
                    await context.Response.WriteAsJsonAsync(errorResponse);
                }
            });
        });
    }

    private static void ConfigureRouting(IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}

