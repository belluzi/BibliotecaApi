namespace BibliotecaApi.UseCases.Livro.DTO
{
    public class ListarLivrosOutputDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string ISBN { get; set; }
    }
}