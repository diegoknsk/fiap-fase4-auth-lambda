using FastFood.Auth.Application.OutputModels.Admin;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Application.Presenters.Admin;
using Microsoft.Extensions.Logging;

namespace FastFood.Auth.Application.UseCases.Admin;

/// <summary>
/// UseCase para executar migrations pendentes no banco de dados.
/// </summary>
public class RunMigrationsUseCase(
    IMigrationService migrationService,
    ILogger<RunMigrationsUseCase> logger)
{
    /// <summary>
    /// Executa as migrations pendentes no banco de dados.
    /// </summary>
    public async Task<RunMigrationsOutputModel> ExecuteAsync()
    {
        logger.LogInformation("Verificando migrations pendentes...");

        // Verificar migrations pendentes
        var pendingMigrations = await migrationService.GetPendingMigrationsAsync();
        var pendingList = pendingMigrations.ToList();

        if (pendingList.Count > 0)
        {
            logger.LogInformation("Encontradas {Count} migration(s) pendente(s): {Migrations}",
                pendingList.Count, string.Join(", ", pendingList));

            // Aplicar migrations
            logger.LogInformation("Aplicando migrations...");
            await migrationService.ApplyMigrationsAsync();
            logger.LogInformation("Migrations aplicadas com sucesso!");
        }
        else
        {
            logger.LogInformation("Nenhuma migration pendente. Banco de dados está atualizado.");
        }

        // Obter lista de migrations aplicadas
        var appliedMigrations = await migrationService.GetAppliedMigrationsAsync();
        var appliedList = appliedMigrations.ToList();

        var outputModel = new RunMigrationsOutputModel
        {
            Success = true,
            PendingMigrationsCount = pendingList.Count,
            PendingMigrations = pendingList,
            AppliedMigrationsCount = appliedList.Count,
            AppliedMigrations = appliedList,
            Message = pendingList.Count > 0
                ? $"✓ {pendingList.Count} migration(s) aplicada(s) com sucesso!"
                : "✓ Nenhuma migration pendente. Banco de dados está atualizado."
        };

        // Chamar Presenter para transformar OutputModel em ResponseModel
        return RunMigrationsPresenter.Present(outputModel);
    }
}


