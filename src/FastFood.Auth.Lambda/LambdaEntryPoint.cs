using Amazon.Lambda.AspNetCoreServer;
using Amazon.Lambda.AspNetCoreServer.Hosting;
using Microsoft.EntityFrameworkCore;
using FastFood.Auth.Infra.Persistence;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Infra.Persistence.Repositories;
using FastFood.Auth.Infra.Services;
using FastFood.Auth.Application.UseCases.Customer;
using FastFood.Auth.Application.UseCases.Admin;
using Microsoft.Extensions.Logging;

namespace FastFood.Auth.Lambda;

/// <summary>
/// Entry point para AWS Lambda usando ASP.NET Core
/// Esta classe é necessária para a imagem base public.ecr.aws/lambda/dotnet:8
/// que requer um handler no formato Assembly::Namespace.Class::Method
/// </summary>
public class LambdaEntryPoint : APIGatewayHttpApiV2ProxyFunction
{
    /// <summary>
    /// Configura o builder da aplicação ASP.NET Core
    /// </summary>
    protected override void Init(IWebHostBuilder builder)
    {
        builder
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseStartup<Startup>();
    }
}

/// <summary>
/// Startup class que replica a configuração do Program.cs
/// Necessária para compatibilidade com APIGatewayHttpApiV2ProxyFunction
/// </summary>
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Configurar hosting Lambda para AWS
        services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

        // Add services to the container
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // Configurar DbContext com PostgreSQL
        // A connection string vem da variável de ambiente ConnectionStrings__DefaultConnection
        services.AddDbContext<AuthDbContext>(options =>
        {
            var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
                ?? throw new InvalidOperationException("ConnectionStrings__DefaultConnection não configurado");
            options.UseNpgsql(connectionString);
        });

        // Registrar repositórios e serviços
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ICognitoService, CognitoService>();

        // Registrar UseCases
        services.AddScoped<CreateAnonymousCustomerUseCase>();
        services.AddScoped<RegisterCustomerUseCase>();
        services.AddScoped<IdentifyCustomerUseCase>();
        services.AddScoped<AuthenticateAdminUseCase>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Configure the HTTP request pipeline
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Tratamento global de exceções
        app.UseExceptionHandler(appBuilder =>
        {
            appBuilder.Run(async context =>
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                
                var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                var exception = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;
                
                if (exception != null)
                {
                    logger.LogError(exception, "Erro não tratado na aplicação");
                    await context.Response.WriteAsJsonAsync(new
                    {
                        message = "Erro interno do servidor",
                        error = env.IsDevelopment() ? exception.Message : null
                    });
                }
            });
        });

        app.UseAuthorization();
        app.MapControllers();
    }
}

