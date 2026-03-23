using BibliotecaApi.Application.Api.Responses;
using BibliotecaApi.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BibliotecaApi.Application.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
    }

    [HttpPost("generate-token")]
    [SwaggerOperation(Summary = "Gera um novo token JWT de autenticação Bearer com email e senha")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ApiResponse<string>))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, Description = "Email ou senha inválidos")]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    public IActionResult GerarToken([FromBody] GerarTokenRequest request)
    {
        try
        {
            var token = _authService.GerarTokenAsync(request);
            return Ok(ApiResponse<string>.Ok(token));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ApiResponse<string>.Falha(ex.Message));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<string>.Falha($"Erro ao gerar token: {ex.Message}"));
        }
    }
}

public class GerarTokenRequest
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}
