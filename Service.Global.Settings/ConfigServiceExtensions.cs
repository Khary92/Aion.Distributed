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
            .AddJsonFile("settings.json", false, true)
            .AddEnvironmentVariables();

        builder.Services
            .AddOptions<GlobalSettings>()
            .Bind(builder.Configuration.GetSection("GlobalSettings"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder.Services
            .AddOptions<AdminSettings>()
            .Bind(builder.Configuration.GetSection("AdminSettings"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder.Services
            .AddOptions<DatabaseSettings>()
            .Bind(builder.Configuration.GetSection("DatabaseSettings"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder.Services
            .AddOptions<MonitoringSettings>()
            .Bind(builder.Configuration.GetSection("MonitoringSettings"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder.Services
            .AddOptions<ServerSettings>()
            .Bind(builder.Configuration.GetSection("ServerSettings"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder.Services.AddUrlBuilder();
    }

    public static void SetConfiguration(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureAppConfiguration((_, config) =>
        {
            config.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("settings.json", false, true)
                .AddEnvironmentVariables();
        });

        hostBuilder.ConfigureServices((context, services) =>
        {
            services.AddOptions<GlobalSettings>()
                .Bind(context.Configuration.GetSection("GlobalSettings"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<AdminSettings>()
                .Bind(context.Configuration.GetSection("AdminSettings"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<ServerSettings>()
                .Bind(context.Configuration.GetSection("ServerSettings"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<MonitoringSettings>()
                .Bind(context.Configuration.GetSection("MonitoringSettings"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<DatabaseSettings>()
                .Bind(context.Configuration.GetSection("DatabaseSettings"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddUrlBuilder();
        });
    }

    private static void AddUrlBuilder(this IServiceCollection services)
    {
        services.AddSingleton<IGrpcUrlService, GrpcUrlService>();
    }
}