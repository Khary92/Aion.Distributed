using Microsoft.Extensions.Configuration;

namespace Core.Persistence;

public static class DatabaseConfiguration
{
    private static IConfiguration GetConfiguration()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        
        return new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
    }
    
    public static string GetConnectionString()
    {
        var configuration = GetConfiguration();
        return configuration.GetConnectionString("DefaultConnection") 
               ?? throw new InvalidOperationException("DefaultConnection string is not configured");
    }
}