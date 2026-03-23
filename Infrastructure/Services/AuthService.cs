using System.Security.Claims;
using BibliotecaApi.Application.Api.Controllers;

namespace BibliotecaApi.Infrastructure.Services;

public class AuthService
{
    private readonly JwtService _jwtService;

    private const string ADMIN_EMAIL = "admin@teste.com";
    private const string ADMIN_SENHA = "bibliotecaapiteste";
    private const int ADMIN_ID = 1;
    private const string ADMIN_NOME = "admin";

    public AuthService(JwtService jwtService)
    {
        _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
    }

    public string GerarTokenAsync(GerarTokenRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
            throw new Exception("O email é obrigatório.");

        if (string.IsNullOrWhiteSpace(request.Senha))
            throw new Exception("A senha é obrigatória.");

        // Validar credenciais contra seed
        if (request.Email != ADMIN_EMAIL || request.Senha != ADMIN_SENHA)
            throw new UnauthorizedAccessException("Email ou senha inválidos.");

        // Credenciais válidas - gerar token com dados do admin
        return _jwtService.GerarToken(ADMIN_ID, ADMIN_NOME, ADMIN_EMAIL);
    }

    public bool ValidarTokenAsync(string? token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return false;

        var principal = _jwtService.ValidarToken(token);
        return principal != null;
    }

    public ClaimsPrincipal? ObterClaimsToken(string? token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return null;

        return _jwtService.ValidarToken(token);
    }
}
