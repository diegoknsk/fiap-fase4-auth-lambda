using Microsoft.EntityFrameworkCore;
using FastFood.Auth.Infra.Persistence;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Infra.Persistence.Repositories;
using FastFood.Auth.Infra.Services;
using FastFood.Auth.Application.UseCases.Customer;
using FastFood.Auth.Application.UseCases.Admin;
using Amazon.Lambda.AspNetCoreServer.Hosting;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Configurar logging
// O Lambda runtime captura automaticamente logs do Console
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

// Configurar hosting Lambda para AWS (substitui Kestrel em produção)
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar DbContext com PostgreSQL
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar repositórios e serviços
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICognitoService, CognitoService>();

// Registrar UseCases
builder.Services.AddScoped<CreateAnonymousCustomerUseCase>();
builder.Services.AddScoped<RegisterCustomerUseCase>();
builder.Services.AddScoped<IdentifyCustomerUseCase>();
builder.Services.AddScoped<AuthenticateAdminUseCase>();

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
