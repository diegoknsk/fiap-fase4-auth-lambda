using Microsoft.AspNetCore.Mvc;
using FastFood.Auth.Application.UseCases.Customer;

namespace FastFood.Auth.Lambda.Controllers;

/// <summary>
/// Controller para operações relacionadas a Customer.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly CreateAnonymousCustomerUseCase _useCase;

    public CustomerController(CreateAnonymousCustomerUseCase useCase)
    {
        _useCase = useCase;
    }

    /// <summary>
    /// Cria um customer anônimo e retorna um token JWT válido para autenticação.
    /// </summary>
    /// <returns>Token JWT e informações do customer criado</returns>
    /// <response code="200">Customer anônimo criado com sucesso</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPost("anonymous")]
    [ProducesResponseType(typeof(Application.Responses.Customer.CreateAnonymousCustomerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAnonymous()
    {
        try
        {
            var result = await _useCase.ExecuteAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            // Log do erro (pode ser melhorado com ILogger)
            return StatusCode(500, new { message = "Erro ao criar customer anônimo", error = ex.Message });
        }
    }
}

