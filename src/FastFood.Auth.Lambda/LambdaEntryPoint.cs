using Amazon.Lambda.AspNetCoreServer;
using Amazon.Lambda.AspNetCoreServer.Hosting;
using Microsoft.EntityFrameworkCore;
using FastFood.Auth.Infra.Persistence;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Infra.Persistence.Repositories;
using FastFood.Auth.Infra.Persistence.Services;
using FastFood.Auth.Infra.Services;
using FastFood.Auth.Application.UseCases.Customer;
using FastFood.Auth.Application.UseCases.Admin;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

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
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Configurar hosting Lambda para AWS
        services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

        // Determinar qual modo a Lambda está executando (Customer, Admin ou Migrator)
        // Default é "Customer" para manter compatibilidade
        var lambdaMode = _configuration["LAMBDA_MODE"] ?? "Customer";
        var isCustomerMode = lambdaMode.Equals("Customer", StringComparison.OrdinalIgnoreCase);
        var isAdminMode = lambdaMode.Equals("Admin", StringComparison.OrdinalIgnoreCase);
        var isMigratorMode = lambdaMode.Equals("Migrator", StringComparison.OrdinalIgnoreCase);

        // Add services to the container
        // Filtrar controllers baseado no modo da Lambda
        services.AddControllers(options =>
        {
            // Adicionar convenção para filtrar controllers baseado no modo
            options.Conventions.Add(new ControllerFilterConvention(isCustomerMode, isAdminMode, isMigratorMode));
        });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // Configurar DbContext com PostgreSQL
        // A connection string vem da variável de ambiente ConnectionStrings__DefaultConnection
        services.AddDbContext<AuthDbContext>(options =>
            options.UseNpgsql(_configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("ConnectionStrings__DefaultConnection não configurado")));

        // Registrar repositórios e serviços comuns
        services.AddScoped<ITokenService, TokenService>();

        // Registrar serviços específicos por modo
        if (isCustomerMode)
        {
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            // Registrar UseCases de Customer
            services.AddScoped<CreateAnonymousCustomerUseCase>();
            services.AddScoped<RegisterCustomerUseCase>();
            services.AddScoped<IdentifyCustomerUseCase>();
        }

        if (isAdminMode)
        {
            services.AddScoped<ICognitoService, CognitoService>();
            // Registrar UseCases de Admin
            services.AddScoped<AuthenticateAdminUseCase>();
        }

        if (isMigratorMode)
        {
            services.AddScoped<IMigrationService, MigrationService>();
            // Registrar UseCases de Migrator
            services.AddScoped<RunMigrationsUseCase>();
        }
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
                
                var logger = context.RequestServices.GetRequiredService<ILogger<Startup>>();
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

        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}

/// <summary>
/// Convenção para filtrar controllers baseado no modo da Lambda (Customer, Admin ou Migrator)
/// </summary>
public class ControllerFilterConvention : IControllerModelConvention
{
    private readonly bool _isCustomerMode;
    private readonly bool _isAdminMode;
    private readonly bool _isMigratorMode;

    public ControllerFilterConvention(bool isCustomerMode, bool isAdminMode, bool isMigratorMode)
    {
        _isCustomerMode = isCustomerMode;
        _isAdminMode = isAdminMode;
        _isMigratorMode = isMigratorMode;
    }

    public void Apply(ControllerModel controller)
    {
        var controllerName = controller.ControllerType.Name;

        // Se estiver em modo Migrator, manter apenas MigrationController
        if (_isMigratorMode)
        {
            if (controllerName != "MigrationController")
            {
                controller.Selectors.Clear();
            }
            return;
        }

        // Se estiver em modo Customer, remover AdminController e MigrationController
        if (_isCustomerMode)
        {
            if (controllerName == "AdminController" || controllerName == "MigrationController")
            {
                controller.Selectors.Clear();
                return;
            }
        }

        // Se estiver em modo Admin, remover CustomerController e MigrationController
        if (_isAdminMode)
        {
            if (controllerName == "CustomerController" || controllerName == "MigrationController")
            {
                controller.Selectors.Clear();
                return;
            }
        }
    }
}

