using Microsoft.Extensions.Logging;

namespace FastFood.Auth.CrossCutting.Extensions;

/// <summary>
/// Extension methods para configuração de logging em Lambdas.
/// </summary>
public static class LoggingBuilderExtensions
{
    /// <summary>
    /// Configura logging padrão para Lambdas (Console, nível Information).
    /// </summary>
    public static ILoggingBuilder AddLambdaLogging(this ILoggingBuilder logging)
    {
        logging.ClearProviders();
        logging.AddConsole();
        logging.SetMinimumLevel(LogLevel.Information);
        
        return logging;
    }
}

