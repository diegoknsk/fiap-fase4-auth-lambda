using FastFood.Auth.Application.Ports;
using FastFood.Auth.Infra.Services;
using FastFood.Auth.Application.UseCases.Admin;
using Amazon.Lambda.AspNetCoreServer.Hosting;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICognitoService, CognitoService>();
builder.Services.AddScoped<AuthenticateAdminUseCase>();

var app = builder.Build();

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

app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program { }
