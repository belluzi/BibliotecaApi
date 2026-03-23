using BibliotecaApi.Domain.Entities;
using BibliotecaApi.Infrastructure.Repositories;
using BibliotecaApi.UseCases.Emprestimo.DTO;

namespace BibliotecaApi.UseCases.Emprestimo;

public class CadastrarEmprestimoUC
{
    private readonly EmprestimoRepository _emprestimoRepository = new EmprestimoRepository();
    private readonly LivroRepository _livroRepository = new LivroRepository();

    public async Task<int> Execute(CadastrarEmprestimoInputDTO input)
    {
        try
        {
            var emprestimo = new EmprestimoEntity();
            emprestimo.Cadastrar(input.IdUsuario, input.IdLivro, input.DataPrevistaDevolucao);

            bool livroDisponivel = await _emprestimoRepository.LivroDisponivel(input.IdLivro);

            if (!livroDisponivel)
            {
                throw new Exception("Este livro já está emprestado e ainda não foi devolvido.");
            }

            bool usuarioAtrasado = await _emprestimoRepository.UsuarioPossuiEmprestimoAtrasadoAsync(input.IdUsuario);

            if (usuarioAtrasado)
            {
                throw new Exception("Usuário com empréstimo em atraso não pode realizar novo empréstimo.");
            }

            int idEmprestimo = await _emprestimoRepository.Cadastrar(emprestimo);

            // Marca o livro como indisponível
            await _livroRepository.MarcarComoIndisponivel(input.IdLivro);

            return idEmprestimo;
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao cadastrar empréstimo: " + ex.Message);
        }
    }
}
