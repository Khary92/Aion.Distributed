using Global.Settings.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Global.Settings;

public static class ConfigServiceExtensions
{
    public static void SetConfiguration(this WebApplicationBuilder builder)
    {
        builder.Configuration
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("settings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
        
        builder.Services.Configure<GlobalSettings>(builder.Configuration.GetSection("GlobalSettings"));
        builder.Services.Configure<AdminSettings>(builder.Configuration.GetSection("AdminSettings"));
        builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
        builder.Services.Configure<MonitoringSettings>(builder.Configuration.GetSection("MonitoringSettings"));
        builder.Services.Configure<ServerSettings>(builder.Configuration.GetSection("ServerSettings"));
    }

    public static IHostBuilder SetConfiguration(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureAppConfiguration((_, config) =>
        {
            config.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("settings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
        });

        hostBuilder.ConfigureServices((context, services) =>
        {
            services.Configure<GlobalSettings>(context.Configuration.GetSection("GlobalSettings"));
            services.Configure<AdminSettings>(context.Configuration.GetSection("AdminSettings"));
            services.Configure<ServerSettings>(context.Configuration.GetSection("ServerSettings"));
            services.Configure<MonitoringSettings>(context.Configuration.GetSection("MonitoringSettings"));
            services.Configure<DatabaseSettings>(context.Configuration.GetSection("DatabaseSettings"));
        });

        return hostBuilder;
    }
}
