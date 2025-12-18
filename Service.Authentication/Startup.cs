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

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlite($"Filename={Path.Combine(Path.GetTempPath(), "openiddict-aridka-server.sqlite3")}");
            options.UseOpenIddict();
        });

        services.AddQuartz(options =>
        {
            options.UseSimpleTypeLoader();
            options.UseInMemoryStore();
        });
        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        var signingRsa = RSA.Create();
        var privateKeyPath = "/certs/private_key_pkcs8.pem";
        var publicKeyPath = "/certs/public_key.pem";

        if (!File.Exists(privateKeyPath))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(privateKeyPath)!);
            signingRsa = RSA.Create(2048);
            File.WriteAllText(privateKeyPath, signingRsa.ExportPkcs8PrivateKeyPem());

            var publicKeyPem = signingRsa.ExportRSAPublicKeyPem();
            File.WriteAllText(publicKeyPath, publicKeyPem);
        }
        else
        {
            signingRsa.ImportFromPem(File.ReadAllText(privateKeyPath));
        }

        var signingKey = new RsaSecurityKey(signingRsa)
        {
            KeyId = "auth-server-signing-key"
        };

        services.AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                    .UseDbContext<ApplicationDbContext>();
                options.UseQuartz();
            })
            .AddServer(options =>
            {
                options.SetTokenEndpointUris("/connect/token");
                options.AllowClientCredentialsFlow();

                options.AddSigningKey(signingKey);

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