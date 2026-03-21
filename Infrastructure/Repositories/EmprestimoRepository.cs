using Dapper;
using BibliotecaApi.Domain.Entities;
using BibliotecaApi.Infrastructure.Data;
using System.Data.Common;

namespace BibliotecaApi.Infrastructure.Repositories;

public class EmprestimoRepository
{
    private readonly DbSession _session;

    public EmprestimoRepository()
    {
        _session = new DbSession(ConfigurationHelper.GetConfiguration());
    }

    public async Task<int> Cadastrar(EmprestimoEntity emprestimo)
    {
        const string sql = @"
            INSERT INTO Emprestimos (id_usuario, id_livro, data_emprestimo, data_prevista_devolucao, valor)
            VALUES (@id_usuario, @id_livro, @data_emprestimo, @data_prevista_devolucao, @valor)
            RETURNING id;
        ";

        var parameters = new
        {
            id_usuario = emprestimo.IdUsuario,
            id_livro = emprestimo.IdLivro,
            data_emprestimo = emprestimo.DataEmprestimo,
            data_prevista_devolucao = emprestimo.DataPrevistaDevolucao,
            valor = emprestimo.Valor
        };

        using (var connection = _session.Connection)
        {
            var result = await _session.Connection.QueryFirstAsync<int>(sql, parameters);
            return result;
        }
    }

    public async Task<bool> Atualizar(EmprestimoEntity emprestimo)
    {
        const string sql = @"
            UPDATE Emprestimos
            SET id_usuario = @id_usuario,
                id_livro = @id_livro,
                data_emprestimo = @data_emprestimo,
                data_prevista_devolucao = @data_prevista_devolucao,
                data_devolucao = @data_devolucao,
                valor = @valor,
                multa = @multa,
                total = @total
            WHERE id = @id;
        ";
        var parameters = new
        {
            id = emprestimo.Id,
            id_usuario = emprestimo.IdUsuario,
            id_livro = emprestimo.IdLivro,
            data_emprestimo = emprestimo.DataEmprestimo,
            data_prevista_devolucao = emprestimo.DataPrevistaDevolucao,
            data_devolucao = emprestimo.DataDevolucao,
            valor = emprestimo.Valor,
            multa = emprestimo.Multa,
            total = emprestimo.Total
        };
        var rowsAffected = await _session.Connection.ExecuteAsync(sql, parameters);
        return rowsAffected > 0;
    }

    public async Task<EmprestimoEntity?> ObterPorIdAsync(int id)
    {
        var sql = "SELECT id, id_usuario idUsuario, id_livro idLivro, data_emprestimo dataEmpretimo, data_prevista_devolucao dataPrevistaDevolucao, data_devolucao dataDevolucao FROM Emprestimos WHERE id = @id";
        return await _session.Connection.QueryFirstOrDefaultAsync<EmprestimoEntity>(sql, new { id });
    }
}
