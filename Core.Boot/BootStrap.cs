using System.Text.Json;
using Core.Persistence;
using Core.Persistence.DbContext;
using Core.Server;
using Core.Server.Tracing;
using Domain.Events.TimerSettings;
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

        builder.Services.AddCoreServices();
        builder.Services.AddInfrastructureServices();
        builder.Services.AddTracingServices();

        builder.Services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo("/app/.aspnet/DataProtection-Keys"));

        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(8080, o => { o.Protocols = HttpProtocols.Http2; });
        });

        builder.Logging.AddConsole();
        builder.Logging.SetMinimumLevel(LogLevel.Debug);

        var app = builder.Build();

        app.UseRouting();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await db.Database.MigrateAsync();

            await SeedAsync(db);
        }

        app.AddEndPoints();
        await app.RunAsync();
    }

    private static async Task SeedAsync(AppDbContext db)
    {
        if (db.TimerSettingsEvents.Any()) return;

        var settingsId = Guid.NewGuid();

        var settingsCreatedEvent = new TimerSettingsCreatedEvent(settingsId, 30, 30);

        db.TimerSettingsEvents.Add(new TimerSettingsEvent(Guid.NewGuid(), DateTimeOffset.Now,
            nameof(TimerSettingsCreatedEvent), settingsId, JsonSerializer.Serialize(settingsCreatedEvent)));

        await db.SaveChangesAsync();
    }
}