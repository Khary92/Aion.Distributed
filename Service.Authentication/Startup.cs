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

        var signingPrivateKeyPath = "/certs/private_signing_key.pem";
        var signingPublicKeyPath = "/certs/public_signing_key.pem";
        RSA signingRsa = RSA.Create();
        if (!File.Exists(signingPrivateKeyPath))
        {
            signingRsa = RSA.Create(2048);
            Directory.CreateDirectory(Path.GetDirectoryName(signingPrivateKeyPath)!);
            Directory.CreateDirectory(Path.GetDirectoryName(signingPublicKeyPath)!);

            // Private Key im PKCS#8 Format
            File.WriteAllText(signingPrivateKeyPath, signingRsa.ExportPkcs8PrivateKeyPem());
            // Public Key im PKCS#8 Format (wichtig!)
            File.WriteAllText(signingPublicKeyPath, signingRsa.ExportSubjectPublicKeyInfoPem());
        }
        else
        {
            signingRsa.ImportFromPem(File.ReadAllText(signingPrivateKeyPath));
        }

        var signingKey = new RsaSecurityKey(signingRsa)
        {
            KeyId = "auth-server-signing-key"
        };

        var encryptionPrivateKeyPath = "/certs/private_encryption_key.pem";
        var encryptionPublicKeyPath = "/certs/public_encryption_key.pem";
        RSA encryptionRsa = RSA.Create();
        if (!File.Exists(encryptionPrivateKeyPath))
        {
            encryptionRsa = RSA.Create(2048);
            Directory.CreateDirectory(Path.GetDirectoryName(encryptionPrivateKeyPath)!);
            Directory.CreateDirectory(Path.GetDirectoryName(encryptionPublicKeyPath)!);

            File.WriteAllText(encryptionPrivateKeyPath, encryptionRsa.ExportPkcs8PrivateKeyPem());
            File.WriteAllText(encryptionPublicKeyPath, encryptionRsa.ExportSubjectPublicKeyInfoPem());
        }
        else
        {
            encryptionRsa.ImportFromPem(File.ReadAllText(encryptionPrivateKeyPath));
        }

        var encryptionKey = new RsaSecurityKey(encryptionRsa)
        {
            KeyId = "auth-server-encryption-key"
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
                options.AddEncryptionKey(encryptionKey);

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