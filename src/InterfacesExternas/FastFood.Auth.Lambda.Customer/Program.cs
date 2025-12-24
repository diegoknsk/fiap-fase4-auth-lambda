using Microsoft.EntityFrameworkCore;
using FastFood.Auth.Infra.Persistence;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Infra.Persistence.Repositories;
using FastFood.Auth.Infra.Persistence.Services;
using FastFood.Auth.Infra.Services;
using FastFood.Auth.Application.UseCases.Customer;
using Amazon.Lambda.AspNetCoreServer.Hosting;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

SetupLogging(builder.Logging);
SetupApplicationServices(builder.Services, builder.Configuration);

var app = builder.Build();

SetupApplicationMiddleware(app);

await app.RunAsync();

static void SetupLogging(ILoggingBuilder logging)
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.SetMinimumLevel(LogLevel.Information);
}

static void SetupApplicationServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    
    var dbConnectionString = configuration.GetConnectionString("DefaultConnection");
    if (!string.IsNullOrEmpty(dbConnectionString))
    {
        services.AddDbContext<AuthDbContext>(options =>
            options.UseNpgsql(dbConnectionString));
    }

    services.AddScoped<ICustomerRepository, CustomerRepository>();
    services.AddScoped<ITokenService, TokenService>();
    services.AddScoped<CreateAnonymousCustomerUseCase>();
    services.AddScoped<RegisterCustomerUseCase>();
    services.AddScoped<IdentifyCustomerUseCase>();
}

static void SetupApplicationMiddleware(WebApplication app)
{
    app.UseExceptionHandler(appBuilder =>
    {
        appBuilder.Run(async context =>
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            
            var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
            var exceptionHandler = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
            var error = exceptionHandler?.Error;
            
            if (error != null)
            {
                logger.LogError(error, "Erro não tratado na aplicação");
                var developmentMode = app.Environment.IsDevelopment();
                var errorResponse = new
                {
                    message = "Erro interno do servidor",
                    error = developmentMode ? error.Message : null
                };
                await context.Response.WriteAsJsonAsync(errorResponse);
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
