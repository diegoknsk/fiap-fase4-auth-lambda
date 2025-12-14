using Microsoft.AspNetCore.Mvc;
using FastFood.Auth.Application.UseCases.Admin;
using FastFood.Auth.Application.Commands.Admin;
using FastFood.Auth.Lambda.Models.Admin;

namespace FastFood.Auth.Lambda.Controllers;

/// <summary>
/// Controller para operações relacionadas a Administradores.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly AuthenticateAdminUseCase _authenticateUseCase;

    public AdminController(AuthenticateAdminUseCase authenticateUseCase)
    {
        _authenticateUseCase = authenticateUseCase;
    }

    /// <summary>
    /// Autentica um administrador através do AWS Cognito usando username e password.
    /// </summary>
    /// <param name="request">Request contendo username e password do administrador</param>
    /// <returns>Tokens de autenticação (AccessToken, IdToken) válidos para uso em outros serviços</returns>
    /// <response code="200">Autenticação bem-sucedida, tokens retornados</response>
    /// <response code="401">Credenciais inválidas</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(Application.Responses.Admin.AuthenticateAdminResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] AdminLoginRequest request)
    {
        try
        {
            var command = new AuthenticateAdminCommand
            {
                Username = request.Username,
                Password = request.Password
            };

            var result = await _authenticateUseCase.ExecuteAsync(command);
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Credenciais inválidas" });
        }
        catch (Exception ex)
        {
            // Log do erro (pode ser melhorado com ILogger)
            return StatusCode(500, new { message = "Erro ao autenticar administrador", error = ex.Message });
        }
    }
}

