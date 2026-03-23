namespace BibliotecaApi.Domain.Entities;

public class EmprestimoEntity
{
    public int Id { get; set; }
    public int IdUsuario { get; private set; }
    public int IdLivro { get; private set; }
    public DateTime DataEmprestimo { get; private set; }
    public DateTime DataPrevistaDevolucao { get; private set; }
    public DateTime? DataDevolucao { get; private set; }
    public Decimal Valor { get; private set; }
    public Decimal Multa { get; private set; }
    public Decimal Total { get; private set; }

    public void Cadastrar(int idUsuario, int idLivro, DateTime dataPrevista)
    {
        if (idUsuario <= 0)
            throw new Exception("Usuário inválido.");

        if (idLivro <= 0)
            throw new Exception("Livro inválido.");

        if (dataPrevista <= DateTime.Now)
            throw new Exception("A data prevista de devolução deve ser futura.");

        IdUsuario = idUsuario;
        IdLivro = idLivro;
        DataEmprestimo = DateTime.Now;
        DataPrevistaDevolucao = dataPrevista;
        DataDevolucao = null;
        Valor = 5.00m; 
    }

    public void RegistrarDevolucao(DateTime dataDevolucao)
    {
        DataDevolucao = dataDevolucao;
        Multa = CalcularMulta();
        Total = Valor + Multa;
    }

    private decimal CalcularMulta()
    {
        if (DataDevolucao == null)
            throw new Exception("Empréstimo ainda não devolvido.");

        if (DataDevolucao.Value <= DataPrevistaDevolucao)
            return 0.00m;

        TimeSpan atraso = DataDevolucao.Value - DataPrevistaDevolucao;
        int diasAtraso = atraso.Days;
        decimal valorMulta = 0.00m;

        int faixaInicialMulta = Math.Min(diasAtraso, 3);
        valorMulta += faixaInicialMulta * 2.00m;

        if (diasAtraso > 3)
        {
            int faixaSecundariaMulta = diasAtraso - 3;
            valorMulta += faixaSecundariaMulta * 3.50m;
        }

        return Math.Min(valorMulta, 50.00m);
        
    }
}
