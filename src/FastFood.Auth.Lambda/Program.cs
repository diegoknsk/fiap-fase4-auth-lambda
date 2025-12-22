using Microsoft.EntityFrameworkCore;
using FastFood.Auth.Infra.Persistence;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Infra.Persistence.Repositories;
using FastFood.Auth.Infra.Services;
using FastFood.Auth.Application.UseCases.Customer;
using FastFood.Auth.Application.UseCases.Admin;
using Amazon.Lambda.AspNetCoreServer.Hosting;

var builder = WebApplication.CreateBuilder(args);

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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// Classe partial para permitir que WebApplicationFactory acesse a aplicação
// Em .NET 8, o compilador gera automaticamente uma classe Program implícita
// Esta declaração partial permite que WebApplicationFactory<Program> funcione
public partial class Program { }
