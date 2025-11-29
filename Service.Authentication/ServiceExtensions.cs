using Microsoft.IdentityModel.Tokens;
using Service.Authorization.Endpoints;
using Service.Authorization.Persistence;
using Service.Authorization.Service;

namespace Service.Authorization;

public static class ServiceExtensions
{
    public static void AddAuthServices(this IServiceCollection services, RsaSecurityKey rsaKey)
    {
        services.InternalServices(rsaKey);
        services.PersistenceServices();
        services.AddEndpoints();
    }
    
    private static void InternalServices(this IServiceCollection services, RsaSecurityKey rsaKey)
    {
        services.AddSingleton<TokenService>(_ => new TokenService(rsaKey));
    }

    private static void PersistenceServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();
    }
    
    private static void AddEndpoints(this IServiceCollection services)
    {
        services.AddScoped<AuthorizationEndpoint>();
        services.AddScoped<TokenEndpoint>();
        services.AddScoped<UserInfoEndpoint>();
    }
}