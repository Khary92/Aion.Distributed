using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.ReactiveUI;
using Client.Desktop.Presentation.Views.Main;
using Client.Tracing;
using Global.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Client;

namespace Client.Desktop;

public static class Program
{
    [STAThread]
    public static async Task Main(string[] args)
    {
        var isMockMode = args.Contains("--mock");

        var hostBuilder = Host.CreateDefaultBuilder(args);
        hostBuilder.SetConfiguration();

        hostBuilder.ConfigureServices((_, services) =>
        {
            services.AddPresentationServices();
            services.AddTracingServices();
            services.AddCommunicationServices(isMockMode);
            services.AddOpenIddict()

                // Register the OpenIddict client components.
                .AddClient(options =>
                {
                    // Allow grant_type=client_credentials to be negotiated.
                    options.AllowClientCredentialsFlow();

                    // Disable token storage, which is not necessary for non-interactive flows like
                    // grant_type=password, grant_type=client_credentials or grant_type=refresh_token.
                    options.DisableTokenStorage();

                    // Register the System.Net.Http integration and use the identity of the current
                    // assembly as a more specific user agent, which can be useful when dealing with
                    // providers that use the user agent as a way to throttle requests (e.g Reddit).
                    options.UseSystemNetHttp()
                        .SetProductInformation(typeof(Program).Assembly);

                    // Add a client registration matching the client application definition in the server project.
                    options.AddRegistration(new OpenIddictClientRegistration
                    {
                        Issuer = new Uri("https://auth.hiegert.eu/", UriKind.Absolute),

                        ClientId = "console",
                        ClientSecret = "388D45FA-B36B-4988-BA59-B187D329C207"
                    });
                });
        });

        var host = hostBuilder.Build();

        await host.StartAsync();

        var serviceProvider = host.Services;

        BuildAvaloniaApp(serviceProvider)
            .StartWithClassicDesktopLifetime(args);

        await host.StopAsync();
    }

    private static AppBuilder BuildAvaloniaApp(IServiceProvider serviceProvider)
    {
        return AppBuilder.Configure(() => new App(serviceProvider))
            .UsePlatformDetect()
            .WithInterFont()
            .UseReactiveUI();
    }
}