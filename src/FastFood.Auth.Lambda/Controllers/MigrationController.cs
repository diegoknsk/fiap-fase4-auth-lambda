using Microsoft.AspNetCore.Mvc;
using FastFood.Auth.Application.UseCases.Admin;
using FastFood.Auth.Application.OutputModels.Admin;

namespace FastFood.Auth.Lambda.Controllers;

/// <summary>
/// Controller para executar migrations do banco de dados.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MigrationController(
    RunMigrationsUseCase runMigrationsUseCase) : ControllerBase
{
    /// <summary>
    /// Executa migrations pendentes no banco de dados.
    /// </summary>
    /// <returns>Informações sobre as migrations aplicadas</returns>
    /// <response code="200">Migrations executadas com sucesso</response>
    /// <response code="500">Erro ao executar migrations</response>
    [HttpPost]
    [ProducesResponseType(typeof(RunMigrationsOutputModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RunMigrations()
    {
        try
        {
            var response = await runMigrationsUseCase.ExecuteAsync();
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new RunMigrationsOutputModel
            {
                Success = false,
                Message = $"Erro ao executar migrations: {ex.Message}",
                Error = ex.Message
            });
        }
    }
}

