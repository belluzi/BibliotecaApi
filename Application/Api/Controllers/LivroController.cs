using BibliotecaApi.Application.Api.Responses;
using BibliotecaApi.UseCases.Livro;
using BibliotecaApi.UseCases.Livro.DTO;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BibliotecaApi.Application.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class LivroController : Controller
{
    private readonly CadastrarLivroUC _cadastrarLivroUC = new CadastrarLivroUC();
    private readonly ListarLivrosUC _listarLivrosUC = new ListarLivrosUC();

 
    [HttpPost]
    [SwaggerOperation(Summary = "Adiciona um novo livro retornando o seu respectivo Id")]
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(ApiResponse<int>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Cadastrar(CadastrarLivroInputDTO input)
    {
        try
        {
            int newId = await _cadastrarLivroUC.Execute(input);
            return Ok(ApiResponse<int>.Ok(newId));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<int>.Falha(ex.Message));
        }

    }

    [HttpGet]
    [SwaggerOperation(Summary = "Lista os livros cadastrados")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ApiResponse<IEnumerable<ListarLivrosOutputDTO>>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Listar()
    {
        try
        {
            var livros = await _listarLivrosUC.Execute();
            var livrosDTO = livros.Select(livro => new ListarLivrosOutputDTO
            {
                Id = livro.Id ?? 0,
                Titulo = livro.Titulo,
                Autor = livro.Autor,
                ISBN = livro.ISBN
            });
            
            return Ok(ApiResponse<IEnumerable<ListarLivrosOutputDTO>>.Ok(livrosDTO));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<IEnumerable<ListarLivrosOutputDTO>>.Falha(ex.Message));
        }
    }
}
