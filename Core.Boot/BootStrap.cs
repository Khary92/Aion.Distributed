using Core.Persistence;
using Core.Persistence.SQLite.DbContext;
using Core.Server;
using Core.Server.Services.UseCase;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Core.Boot;
public class BootStrap
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddGrpc(options =>
        {
            options.EnableDetailedErrors = true;
            options.MaxReceiveMessageSize = 2 * 1024 * 1024; // 2 MB
            options.MaxSendMessageSize = 2 * 1024 * 1024;    // 2 MB
        });
        
        builder.Services.AddCoreServices();
        builder.Services.AddInfrastructureServices();

        // Kestrel-Konfiguration
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(8080, o => 
            {
                o.Protocols = HttpProtocols.Http2;
            });
        });

        // Logging hinzufügen
        builder.Logging.AddConsole();
        builder.Logging.SetMinimumLevel(LogLevel.Debug);

        var app = builder.Build();

        // Middleware in der richtigen Reihenfolge
        app.UseRouting();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await db.Database.MigrateAsync();
            scope.ServiceProvider.GetRequiredService(typeof(TimerService));
        }

        app.AddEndPoints();

        app.Lifetime.ApplicationStarted.Register(() =>
        {
            var endpointDataSource = app.Services.GetRequiredService<EndpointDataSource>();
            foreach (var endpoint in endpointDataSource.Endpoints)
            {
                if (endpoint.DisplayName!.Contains("Unimplemented"))
                {
                    continue;
                }
                Console.WriteLine($"[Endpoint] {endpoint.DisplayName}");
            }
        });

        await app.RunAsync();
    }
}