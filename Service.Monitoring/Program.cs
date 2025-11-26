using System.Security.Cryptography.X509Certificates;
using Global.Settings;
using Global.Settings.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Service.Monitoring;

public abstract class Program
{
    [STAThread]
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.SetConfiguration();

        builder.Services.AddGrpc(options =>
        {
            options.EnableDetailedErrors = true;
            options.MaxReceiveMessageSize = 2 * 1024 * 1024;
            options.MaxSendMessageSize = 2 * 1024 * 1024;
        });

        builder.Services.AddTracingServices();

        var globalSettings = new GlobalSettings();
        builder.Configuration.GetSection("GlobalSettings").Bind(globalSettings);

        var monitoringSettings = new MonitoringSettings();
        builder.Configuration.GetSection("MonitoringSettings").Bind(monitoringSettings);

        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(monitoringSettings.GrpcPort, listenOptions =>
            {
                if (globalSettings.UseHttps)
                {
                    listenOptions.UseHttps(httpsOptions =>
                    {
                        var cert = X509Certificate2.CreateFromPemFile(
                            "/certs/fullchain1.pem",
                            "/certs/privkey1.pem"
                        );

                        httpsOptions.ServerCertificate = cert;
                    });
                }

                listenOptions.Protocols = HttpProtocols.Http2;
            });
        });

        builder.Services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo("/app/DataProtection-Keys"))
            .SetApplicationName("Aion");

        var app = builder.Build();
        app.UseRouting();
        app.AddEndPoints();

        await app.RunAsync();
    }
}