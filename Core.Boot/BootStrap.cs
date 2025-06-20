using Core.Persistence;
using Core.Persistence.SQLite.DbContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Service.Server;
using Service.Server.Services.UseCase;

namespace Core.Boot;

public class BootStrap
{
    public static async Task Main(string[] args) // <- async Task Main
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenLocalhost(5000, o => o.Protocols = HttpProtocols.Http2);
        });

        builder.Services.AddCoreServices();
        builder.Services.AddInfrastructureServices();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await db.Database.MigrateAsync();
            
            //This is required or else the service will not be created
            scope.ServiceProvider.GetRequiredService(typeof(TimerService));
        }

        app.AddEndPoints();

        app.Lifetime.ApplicationStarted.Register(() =>
        {
            var endpointDataSource = app.Services.GetRequiredService<EndpointDataSource>();
            foreach (var endpoint in endpointDataSource.Endpoints)
            {
                Console.WriteLine($"[Endpoint] {endpoint.DisplayName}");
            }
        });

        app.Run();
    }
}