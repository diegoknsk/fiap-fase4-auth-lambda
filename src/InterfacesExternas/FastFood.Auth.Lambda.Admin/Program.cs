using FastFood.Auth.Application.Ports;
using FastFood.Auth.Infra.Services;
using FastFood.Auth.Application.UseCases.Admin;
using Amazon.Lambda.AspNetCoreServer.Hosting;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

ConfigureLogging(builder.Logging);
ConfigureServices(builder.Services);

var app = builder.Build();

ConfigureMiddleware(app);

await app.RunAsync();

static void ConfigureLogging(ILoggingBuilder logging)
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.SetMinimumLevel(LogLevel.Information);
}

static void ConfigureServices(IServiceCollection services)
{
    services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddScoped<ICognitoService, CognitoService>();
    services.AddScoped<AuthenticateAdminUseCase>();
}

static void ConfigureMiddleware(WebApplication app)
{
    app.UseExceptionHandler(appBuilder =>
    {
        appBuilder.Run(async context =>
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            
            var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
            var exceptionFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
            var exception = exceptionFeature?.Error;
            
            if (exception != null)
            {
                logger.LogError(exception, "Erro não tratado na aplicação");
                var isDev = app.Environment.IsDevelopment();
                var response = new
                {
                    message = "Erro interno do servidor",
                    error = isDev ? exception.Message : null
                };
                await context.Response.WriteAsJsonAsync(response);
            }
        });
    });

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthorization();
    app.MapControllers();
}

public partial class Program { }
