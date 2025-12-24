using Microsoft.EntityFrameworkCore;
using FastFood.Auth.Infra.Persistence;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Infra.Persistence.Repositories;
using FastFood.Auth.Infra.Persistence.Services;
using FastFood.Auth.Infra.Services;
using FastFood.Auth.Application.UseCases.Customer;
using FastFood.Auth.CrossCutting.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddLambdaLogging();
builder.Services.AddLambdaCommonServices();

var dbConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (!string.IsNullOrEmpty(dbConnectionString))
{
    builder.Services.AddDbContext<AuthDbContext>(options =>
        options.UseNpgsql(dbConnectionString));
}

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<CreateAnonymousCustomerUseCase>();
builder.Services.AddScoped<RegisterCustomerUseCase>();
builder.Services.AddScoped<IdentifyCustomerUseCase>();

var app = builder.Build();

app.UseSwaggerInDevelopment(app.Environment);
app.UseGlobalExceptionHandler(app.Environment);
app.UseDefaultRouting();

await app.RunAsync();

public partial class Program { }
