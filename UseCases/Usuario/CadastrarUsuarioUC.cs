using BibliotecaApi.Domain.Entities;
using BibliotecaApi.Infrastructure.Repositories;
using BibliotecaApi.UseCases.Usuario.DTO;

namespace BibliotecaApi.UseCases.Usuario;

public class CadastrarUsuarioUC
{
    private UsuarioEntity _usuario = new UsuarioEntity();
    private UsuarioRepository _repository = new UsuarioRepository();

    public async Task<int> Execute(CadastrarUsuarioInputDTO input)
    {
        try
        {
            _usuario.Cadastrar(input.Nome, input.CPF, input.Email);

            var cpfExistente = await _repository.CpfExistente(input.CPF);

            if (cpfExistente)
            {
                throw new Exception("Usuário com este CPF já está cadastrado.");
            }

            int idNovoUsuario = await _repository.Cadastrar(_usuario);
            return idNovoUsuario;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}