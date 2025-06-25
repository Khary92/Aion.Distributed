using Core.Persistence;
using Core.Persistence.SQLite.DbContext;
using Core.Server;
using Core.Server.Services.UseCase;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

            //This is required or else the service won't be started 
            scope.ServiceProvider.GetRequiredService(typeof(TimerService));
        }

        app.AddEndPoints();
        await app.RunAsync();
    }
}