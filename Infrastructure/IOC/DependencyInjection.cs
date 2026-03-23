
using BibliotecaApi.Infrastructure.Data;
using BibliotecaApi.Infrastructure.Repositories;
using BibliotecaApi.Infrastructure.Services;

namespace MinimalApplication.Infrastructure.IOC;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<JwtService>();
        services.AddSingleton<AuthService>();

        return services;
    }
}
