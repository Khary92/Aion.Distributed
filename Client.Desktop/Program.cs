using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.ReactiveUI;
using Client.Desktop.Presentation.Views.Main;
using Client.Tracing;
using Global.Settings;
using Microsoft.Extensions.Hosting;

namespace Client.Desktop;

public static class Program
{
    [STAThread]
    public static async Task Main(string[] args)
    {
        var hostBuilder = Host.CreateDefaultBuilder(args);
        hostBuilder.SetConfiguration();
        hostBuilder.ConfigureServices((_, services) =>
        {
            services.AddPresentationServices();
            services.AddTracingServices();
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