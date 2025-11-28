using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using Core.Persistence;
using Core.Persistence.DbContext;
using Core.Server;
using Core.Server.Tracing;
using Domain.Events.TimerSettings;
using Global.Settings;
using Global.Settings.Types;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
        
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = "your-issuer",
                    ValidateAudience = true,
                    ValidAudience = "your-audience",
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey("super-secret-key"u8.ToArray()),
                    ValidateLifetime = true,
                };

                // Optional: support access_token in querystring for gRPC-Web or web clients
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        // if header is missing, allow token via ?access_token=... (useful for gRPC-web)
                        if (string.IsNullOrEmpty(context.Request.Headers["Authorization"]))
                        {
                            if (context.Request.Query.TryGetValue("access_token", out var t))
                                context.Token = t;
                        }

                        return Task.CompletedTask;
                    }
                };
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
        var serverSettings = new ServerSettings();
        builder.Configuration.GetSection("ServerSettings").Bind(serverSettings);

        builder.WebHost.ConfigureKestrel(options =>
        {
            // Internal GRPC listener (HTTP/2, no TLS)
            options.ListenAnyIP(serverSettings.InternalGrpcPort, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http2;
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            });

            // External GRPC listener (HTTPS + HTTP/2)
            options.ListenAnyIP(serverSettings.SecureExternalGrpcPort, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http2;

                listenOptions.UseHttps(httpsOptions =>
                {
                    var cert = X509Certificate2.CreateFromPemFile(
                        "/certs/fullchain1.pem",
                        "/certs/privkey1.pem"
                    );

                    httpsOptions.ServerCertificate = cert;
                });
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