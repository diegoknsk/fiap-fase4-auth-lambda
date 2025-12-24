using FastFood.Auth.Application.Ports;
using FastFood.Auth.Infra.Services;
using FastFood.Auth.Application.UseCases.Admin;
using FastFood.Auth.CrossCutting.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddLambdaLogging();
builder.Services.AddLambdaCommonServices();
builder.Services.AddScoped<ICognitoService, CognitoService>();
builder.Services.AddScoped<AuthenticateAdminUseCase>();

var app = builder.Build();

app.UseSwaggerInDevelopment(app.Environment);
app.UseGlobalExceptionHandler(app.Environment);
app.UseDefaultRouting();

await app.RunAsync();

public partial class Program { }
