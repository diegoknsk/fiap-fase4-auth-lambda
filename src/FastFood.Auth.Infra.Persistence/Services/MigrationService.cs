using Microsoft.EntityFrameworkCore;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Infra.Persistence;

namespace FastFood.Auth.Infra.Persistence.Services;

/// <summary>
/// Implementação do serviço de migrations usando Entity Framework Core.
/// Responsável por executar migrations no banco de dados.
/// </summary>
public class MigrationService(AuthDbContext context) : IMigrationService
{
    public async Task<IEnumerable<string>> GetPendingMigrationsAsync()
    {
        return await context.Database.GetPendingMigrationsAsync();
    }

    public async Task ApplyMigrationsAsync()
    {
        await context.Database.MigrateAsync();
    }

    public async Task<IEnumerable<string>> GetAppliedMigrationsAsync()
    {
        return await context.Database.GetAppliedMigrationsAsync();
    }
}

