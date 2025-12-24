using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FastFood.Auth.CrossCutting.Extensions;

/// <summary>
/// Extension methods para configuração de middleware comuns em Lambdas.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Configura o Swagger apenas em ambiente de desenvolvimento.
    /// </summary>
    public static IApplicationBuilder UseSwaggerInDevelopment(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.EnvironmentName == "Development")
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        return app;
    }

    /// <summary>
    /// Configura tratamento global de exceções com resposta JSON padronizada.
    /// </summary>
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseExceptionHandler(appBuilder =>
        {
            appBuilder.Run(async context =>
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                
                var loggerFactory = context.RequestServices.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger("GlobalExceptionHandler");
                var exceptionFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
                var exception = exceptionFeature?.Error;
                
                if (exception != null)
                {
                    logger.LogError(exception, "Erro não tratado na aplicação");
                    var isDevelopment = env.EnvironmentName == "Development";
                    var errorResponse = new
                    {
                        message = "Erro interno do servidor",
                        error = isDevelopment ? exception.Message : null
                    };
                    var json = System.Text.Json.JsonSerializer.Serialize(errorResponse);
                    await context.Response.WriteAsync(json);
                }
            });
        });
        
        return app;
    }

    /// <summary>
    /// Configura roteamento padrão com autorização e mapeamento de controllers.
    /// </summary>
    public static IApplicationBuilder UseDefaultRouting(this IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        
        return app;
    }
}

