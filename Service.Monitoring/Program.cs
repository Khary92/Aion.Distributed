using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Service.Monitoring;

public abstract class Program
{
    [STAThread]
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddGrpc(options =>
        {
            options.EnableDetailedErrors = true;
            options.MaxReceiveMessageSize = 2 * 1024 * 1024;
            options.MaxSendMessageSize = 2 * 1024 * 1024;
        });

        builder.Services.AddTracingServices();

        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(8080, o => { o.Protocols = HttpProtocols.Http2; });
        });

        // builder.Logging.AddConsole();
        // builder.Logging.SetMinimumLevel(LogLevel.Debug);

        var app = builder.Build();

        app.UseRouting();

        app.AddEndPoints();
        await app.RunAsync();
    }
}