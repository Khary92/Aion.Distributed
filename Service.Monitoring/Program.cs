using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Global.Settings;
using Global.Settings.Types;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Tokens;

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


        var publicKeyPath = "/jwt/public_key.pem";

        var rsa = RSA.Create();
        rsa.ImportFromPem(await File.ReadAllTextAsync(publicKeyPath));

        var rsaKey = new RsaSecurityKey(rsa)
        {
            KeyId = "auth-server-key"
        };

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = "http://localhost:5001",
                    ValidateAudience = true,
                    ValidAudience = "api",
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = rsaKey,
                    ValidateLifetime = true
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (string.IsNullOrEmpty(context.Request.Headers["Authorization"]) &&
                            context.Request.Query.TryGetValue("access_token", out var t))
                        {
                            context.Token = t;
                        }

                        return Task.CompletedTask;
                    }
                };
            });


        var monitoringSettings = new MonitoringSettings();
        builder.Configuration.GetSection("MonitoringSettings").Bind(monitoringSettings);

        builder.WebHost.ConfigureKestrel(options =>
        {
            // Internal GRPC listener (HTTP/2, no TLS)
            options.ListenAnyIP(monitoringSettings.InternalGrpcPort, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http2;
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            });

            // External GRPC listener (HTTPS + HTTP/2)
            options.ListenAnyIP(monitoringSettings.SecureExternalGrpcPort, listenOptions =>
            {
                listenOptions.UseHttps(httpsOptions =>
                {
                    var cert = X509Certificate2.CreateFromPemFile(
                        "/certs/fullchain1.pem",
                        "/certs/privkey1.pem"
                    );

                    httpsOptions.ServerCertificate = cert;
                });

                listenOptions.Protocols = HttpProtocols.Http2;
            });
        });

        var app = builder.Build();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization(); 
        
        app.AddEndPoints();
        
        await app.RunAsync();
    }
}