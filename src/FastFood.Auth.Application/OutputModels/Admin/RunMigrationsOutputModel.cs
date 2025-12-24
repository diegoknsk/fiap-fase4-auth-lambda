namespace FastFood.Auth.Application.OutputModels.Admin;

/// <summary>
/// OutputModel do UseCase RunMigrationsUseCase contendo informações sobre as migrations executadas.
/// </summary>
public class RunMigrationsOutputModel
{
    /// <summary>
    /// Indica se a operação foi bem-sucedida
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Mensagem de resultado
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Quantidade de migrations pendentes encontradas
    /// </summary>
    public int PendingMigrationsCount { get; set; }

    /// <summary>
    /// Lista de migrations pendentes
    /// </summary>
    public List<string> PendingMigrations { get; set; } = new();

    /// <summary>
    /// Quantidade de migrations aplicadas no banco
    /// </summary>
    public int AppliedMigrationsCount { get; set; }

    /// <summary>
    /// Lista de migrations aplicadas no banco
    /// </summary>
    public List<string> AppliedMigrations { get; set; } = new();

    /// <summary>
    /// Mensagem de erro (se houver)
    /// </summary>
    public string? Error { get; set; }
}

