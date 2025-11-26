using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Core.Persistence;
using Core.Persistence.DbContext;
using Core.Server;
using Core.Server.Tracing;
using Domain.Events.TimerSettings;
using Global.Settings;
using Global.Settings.Types;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;

namespace Core.Boot;

public static class BootStrap
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddGrpc(options =>
        {
            options.EnableDetailedErrors = true;
            options.MaxReceiveMessageSize = 2 * 1024 * 1024;
            options.MaxSendMessageSize = 2 * 1024 * 1024;
        });

        builder.SetConfiguration();
        builder.Services.AddCoreServices();
        builder.Services.AddInfrastructureServices();
        builder.Services.AddTracingServices();
        
        builder.Services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo("/app/DataProtection-Keys"))
            .SetApplicationName("Aion");
        
        SetupKestrel(builder);

        builder.Logging.AddConsole();
        builder.Logging.SetMinimumLevel(LogLevel.Debug);

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await db.Database.MigrateAsync();
            await SeedAsync(db);
        }

        app.AddEndPoints();
        app.UseRouting();

        await app.RunAsync();
    }

    private static void SetupKestrel(WebApplicationBuilder builder)
    {
        var globalSettings = new GlobalSettings();
        builder.Configuration.GetSection("GlobalSettings").Bind(globalSettings);

        var serverSettings = new ServerSettings();
        builder.Configuration.GetSection("ServerSettings").Bind(serverSettings);

        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(serverSettings.GrpcPort, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http2;

                if (globalSettings.UseHttps)
                {
                    listenOptions.UseHttps(httpsOptions =>
                    {
                        var cert = X509Certificate2.CreateFromPemFile(
                            "/certs/fullchain.pem",
                            "/certs/privkey.pem"
                        );

                        httpsOptions.ServerCertificate = cert;
                    });
                }

                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            });
        });
    }

    private static async Task SeedAsync(AppDbContext db)
    {
        if (db.TimerSettingsEvents.Any()) return;

        var settingsId = Guid.NewGuid();
        var settingsCreatedEvent = new TimerSettingsCreatedEvent(settingsId, 30, 30);

        db.TimerSettingsEvents.Add(new TimerSettingsEvent(
            Guid.NewGuid(),
            DateTimeOffset.Now,
            nameof(TimerSettingsCreatedEvent),
            settingsId,
            JsonSerializer.Serialize(settingsCreatedEvent)
        ));

        await db.SaveChangesAsync();
    }
}