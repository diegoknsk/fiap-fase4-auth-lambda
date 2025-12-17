using Microsoft.AspNetCore.Mvc;
using FastFood.Auth.Application.UseCases.Customer;
using FastFood.Auth.Application.InputModels.Customer;
using FastFood.Auth.Application.OutputModels.Customer;
using FastFood.Auth.Application.Presenters.Customer;
using FastFood.Auth.Lambda.Models.Customer;

namespace FastFood.Auth.Lambda.Controllers;

/// <summary>
/// Controller para operações relacionadas a Customer.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly CreateAnonymousCustomerUseCase _createAnonymousUseCase;
    private readonly RegisterCustomerUseCase _registerUseCase;
    private readonly IdentifyCustomerUseCase _identifyUseCase;
    private readonly CreateAnonymousCustomerPresenter _createAnonymousPresenter;
    private readonly RegisterCustomerPresenter _registerPresenter;
    private readonly IdentifyCustomerPresenter _identifyPresenter;

    public CustomerController(
        CreateAnonymousCustomerUseCase createAnonymousUseCase,
        RegisterCustomerUseCase registerUseCase,
        IdentifyCustomerUseCase identifyUseCase,
        CreateAnonymousCustomerPresenter createAnonymousPresenter,
        RegisterCustomerPresenter registerPresenter,
        IdentifyCustomerPresenter identifyPresenter)
    {
        _createAnonymousUseCase = createAnonymousUseCase;
        _registerUseCase = registerUseCase;
        _identifyUseCase = identifyUseCase;
        _createAnonymousPresenter = createAnonymousPresenter;
        _registerPresenter = registerPresenter;
        _identifyPresenter = identifyPresenter;
    }

    /// <summary>
    /// Cria um customer anônimo e retorna um token JWT válido para autenticação.
    /// </summary>
    /// <returns>Token JWT e informações do customer criado</returns>
    /// <response code="200">Customer anônimo criado com sucesso</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPost("anonymous")]
    [ProducesResponseType(typeof(CreateAnonymousCustomerOutputModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAnonymous()
    {
        try
        {
            var result = await _createAnonymousUseCase.ExecuteAsync();
            var response = _createAnonymousPresenter.Present(result);
            return Ok(response);
        }
        catch (Exception ex)
        {
            // Log do erro (pode ser melhorado com ILogger)
            return StatusCode(500, new { message = "Erro ao criar customer anônimo", error = ex.Message });
        }
    }

    /// <summary>
    /// Registra um customer através do CPF e retorna um token JWT válido para autenticação.
    /// Se o customer já existir, retorna o token do customer existente.
    /// </summary>
    /// <param name="request">Request contendo o CPF do customer</param>
    /// <returns>Token JWT e informações do customer registrado</returns>
    /// <response code="200">Customer registrado ou identificado com sucesso</response>
    /// <response code="400">CPF inválido</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisterCustomerOutputModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] RegisterCustomerRequest request)
    {
        try
        {
            var inputModel = new RegisterCustomerInputModel
            {
                Cpf = request.Cpf
            };

            var result = await _registerUseCase.ExecuteAsync(inputModel);
            var response = _registerPresenter.Present(result);
            return Ok(response);
        }
        catch (Exception ex)
        {
            // Log do erro (pode ser melhorado com ILogger)
            return StatusCode(500, new { message = "Erro ao registrar customer", error = ex.Message });
        }
    }

    /// <summary>
    /// Identifica um customer existente através do CPF e retorna um token JWT válido para autenticação.
    /// </summary>
    /// <param name="request">Request contendo o CPF do customer</param>
    /// <returns>Token JWT e informações do customer identificado</returns>
    /// <response code="200">Customer identificado com sucesso</response>
    /// <response code="401">Customer não encontrado com o CPF fornecido</response>
    /// <response code="400">CPF inválido</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpPost("identify")]
    [ProducesResponseType(typeof(IdentifyCustomerOutputModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Identify([FromBody] IdentifyCustomerRequest request)
    {
        try
        {
            var inputModel = new IdentifyCustomerInputModel
            {
                Cpf = request.Cpf
            };

            var result = await _identifyUseCase.ExecuteAsync(inputModel);
            var response = _identifyPresenter.Present(result);
            return Ok(response);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Customer not found with the provided CPF." });
        }
        catch (Exception ex)
        {
            // Log do erro (pode ser melhorado com ILogger)
            return StatusCode(500, new { message = "Erro ao identificar customer", error = ex.Message });
        }
    }
}

