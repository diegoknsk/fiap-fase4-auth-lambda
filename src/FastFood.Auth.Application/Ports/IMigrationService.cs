namespace FastFood.Auth.Application.Ports;

/// <summary>
/// Port (interface) para serviço de migrations na camada Application.
/// Define os contratos para execução de migrations do banco de dados.
/// </summary>
public interface IMigrationService
{
    /// <summary>
    /// Obtém a lista de migrations pendentes
    /// </summary>
    Task<IEnumerable<string>> GetPendingMigrationsAsync();

    /// <summary>
    /// Aplica as migrations pendentes no banco de dados
    /// </summary>
    Task ApplyMigrationsAsync();

    /// <summary>
    /// Obtém a lista de migrations já aplicadas no banco de dados
    /// </summary>
    Task<IEnumerable<string>> GetAppliedMigrationsAsync();
}

