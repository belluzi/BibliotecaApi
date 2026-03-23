using BibliotecaApi.Infrastructure.Services;

namespace BibliotecaApi.Infrastructure.Middleware;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    private static readonly HashSet<string> RotasPublicas = new(StringComparer.OrdinalIgnoreCase)
    {
        "/auth/generate-token",
        "/swagger",
        "/swagger/index.html",
        "/swagger/v1/swagger.json",
        "/livro/listar"
    };

    public AuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, AuthService authService)
    {
        var path = context.Request.Path.Value?.ToLower() ?? "";

        if (RotasPublicas.Any(rota => path.Contains(rota.ToLower())))
        {
            await _next(context);
            return;
        }

        if (!RequerAutenticacao(context.Request.Method))
        {
            await _next(context);
            return;
        }

        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        var valid = authService.ValidarTokenAsync(authHeader);

        if (!valid)
        {
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new { erro = "Unauthorized", mensagem = "Token JWT inválido, expirado ou ausente" });
            return;
        }

        var claims = authService.ObterClaimsToken(authHeader);
        if (claims != null)
        {
            context.User = claims;
        }

        await _next(context);
    }

    private static bool RequerAutenticacao(string metodo)
    {
        return metodo.Equals("POST", StringComparison.OrdinalIgnoreCase)
            || metodo.Equals("PUT", StringComparison.OrdinalIgnoreCase)
            || metodo.Equals("DELETE", StringComparison.OrdinalIgnoreCase);
    }
}
