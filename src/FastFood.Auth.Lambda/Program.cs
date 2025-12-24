using Microsoft.EntityFrameworkCore;
using FastFood.Auth.Infra.Persistence;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Infra.Persistence.Repositories;
using FastFood.Auth.Infra.Persistence.Services;
using FastFood.Auth.Infra.Services;
using FastFood.Auth.Application.UseCases.Customer;
using FastFood.Auth.Application.UseCases.Admin;
using Amazon.Lambda.AspNetCoreServer.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

var builder = WebApplication.CreateBuilder(args);

// Configurar logging
// O Lambda runtime captura automaticamente logs do Console
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

// Configurar hosting Lambda para AWS (substitui Kestrel em produção)
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

// Determinar qual modo a Lambda está executando (Customer, Admin ou Migrator)
// Default é "Customer" para manter compatibilidade
var lambdaMode = builder.Configuration["LAMBDA_MODE"] ?? "Customer";
var isCustomerMode = lambdaMode.Equals("Customer", StringComparison.OrdinalIgnoreCase);
var isAdminMode = lambdaMode.Equals("Admin", StringComparison.OrdinalIgnoreCase);
var isMigratorMode = lambdaMode.Equals("Migrator", StringComparison.OrdinalIgnoreCase);

// Add services to the container.
// Filtrar controllers baseado no modo da Lambda
builder.Services.AddControllers(options =>
{
    // Adicionar convenção para filtrar controllers baseado no modo
    options.Conventions.Add(new ControllerFilterConvention(isCustomerMode, isAdminMode, isMigratorMode));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar DbContext com PostgreSQL
// A connection string vem da variável de ambiente ConnectionStrings__DefaultConnection
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("ConnectionStrings__DefaultConnection não configurado")));

// Registrar serviços comuns
builder.Services.AddScoped<ITokenService, TokenService>();

// Registrar serviços específicos por modo
if (isCustomerMode)
{
    builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
    // Registrar UseCases de Customer
    builder.Services.AddScoped<CreateAnonymousCustomerUseCase>();
    builder.Services.AddScoped<RegisterCustomerUseCase>();
    builder.Services.AddScoped<IdentifyCustomerUseCase>();
}

if (isAdminMode)
{
    builder.Services.AddScoped<ICognitoService, CognitoService>();
    // Registrar UseCases de Admin
    builder.Services.AddScoped<AuthenticateAdminUseCase>();
}

if (isMigratorMode)
{
    builder.Services.AddScoped<IMigrationService, MigrationService>();
    // Registrar UseCases de Migrator
    builder.Services.AddScoped<RunMigrationsUseCase>();
}

// Presenters são classes static, não precisam ser registradas no DI

var app = builder.Build();

// Configure the HTTP request pipeline.

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
        error = app.Environment.IsDevelopment() ? exception.Message : null
      });
    }
  });
});

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

// Removido UseHttpsRedirection - Function URL já usa HTTPS e pode causar problemas
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// Classe partial para permitir que WebApplicationFactory acesse a aplicação
// Em .NET 8, o compilador gera automaticamente uma classe Program implícita
// Esta declaração partial permite que WebApplicationFactory<Program> funcione
public partial class Program { }

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
