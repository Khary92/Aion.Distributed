using System.Security.Cryptography;
using Aridka.Server.Models;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quartz;

namespace Aridka.Server;

public class Startup
{
    public Startup(IConfiguration configuration)
        => Configuration = configuration;

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();

        // DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlite($"Filename={Path.Combine(Path.GetTempPath(), "openiddict-aridka-server.sqlite3")}");
            options.UseOpenIddict();
        });

        // Quartz.NET
        services.AddQuartz(options =>
        {
            options.UseSimpleTypeLoader();
            options.UseInMemoryStore();
        });
        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        var keyPath = "/certs/private_key_pkcs8.pem";
        var pubKeyPath = "/certs/public_key.pem";

        RSA rsa;
        if (!File.Exists(keyPath))
        {
            rsa = RSA.Create(2048);

            var privPem = rsa.ExportPkcs8PrivateKeyPem();
            Directory.CreateDirectory(Path.GetDirectoryName(keyPath)!);
            File.WriteAllText(keyPath, privPem);

            var pubPem = rsa.ExportSubjectPublicKeyInfoPem();
            File.WriteAllText(pubKeyPath, pubPem);
        }
        else
        {
            rsa = RSA.Create();
            rsa.ImportFromPem(File.ReadAllText(keyPath));
        }

        var rsaKey = new RsaSecurityKey(rsa)
        {
            KeyId = "auth-server-key"
        };
        
        // OpenIddict
        services.AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                       .UseDbContext<ApplicationDbContext>();
                options.UseQuartz();
            })
            .AddServer(options =>
            {
                options.SetTokenEndpointUris("connect/token");
                options.AllowClientCredentialsFlow();

                options.AddSigningKey(rsaKey);
                options.AddEphemeralEncryptionKey();
                
                options.UseAspNetCore()
                    .EnableTokenEndpointPassthrough();

                options.SetIssuer(new Uri("https://auth.hiegert.eu"));
            })
            .AddValidation(options =>
            {
                options.UseLocalServer();
                options.UseAspNetCore();
            });

        services.AddHostedService<Worker>();
    }

    public void Configure(IApplicationBuilder app)
    {
        var forwardedOptions = new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        };
        forwardedOptions.KnownNetworks.Clear();
        forwardedOptions.KnownProxies.Clear();

        app.UseForwardedHeaders(forwardedOptions);

        app.UseDeveloperExceptionPage();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapDefaultControllerRoute();
        });

        app.UseWelcomePage();
    }
}
