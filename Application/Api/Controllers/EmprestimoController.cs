using BibliotecaApi.Application.Api.Responses;
using BibliotecaApi.UseCases.Emprestimo;
using BibliotecaApi.UseCases.Emprestimo.DTO;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BibliotecaApi.Application.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class EmprestimoController : Controller
{
    private readonly CadastrarEmprestimoUC _cadastrarEmprestimoUC = new CadastrarEmprestimoUC();
    private readonly DevolverEmprestimoUC _devolverEmprestimoUC = new DevolverEmprestimoUC();

    [HttpPost]
    [SwaggerOperation(Summary = "Registra um novo empréstimo de livro")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ApiResponse<int>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Cadastrar([FromBody] CadastrarEmprestimoInputDTO input)
    {
        try
        {
            int idEmprestimo = await _cadastrarEmprestimoUC.Execute(input);
            return Ok(ApiResponse<int>.Ok(idEmprestimo));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<int>.Falha("Erro ao registrar empréstimo: " + ex.Message));
        }
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Registra a devolução de um empréstimo de livro")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ApiResponse<string>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Devolver([FromBody] DevolverEmprestimoInputDTO input)
    {
        try
        {
            string resultado = await _devolverEmprestimoUC.Execute(input);
            return Ok(ApiResponse<string>.Ok(resultado));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<int>.Falha("Erro ao devolver empréstimo: " + ex.Message));
        }
    }
}
