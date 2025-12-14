using Microsoft.EntityFrameworkCore;
using FastFood.Auth.Infra.Persistence;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Infra.Persistence.Repositories;
using FastFood.Auth.Infra.Services;
using FastFood.Auth.Application.UseCases.Customer;
using FastFood.Auth.Application.UseCases.Admin;

var builder = WebApplication.CreateBuilder(args);

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
