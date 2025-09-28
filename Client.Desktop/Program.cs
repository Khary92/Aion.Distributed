using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.ReactiveUI;
using Client.Desktop.Config;
using Client.Desktop.Presentation.Views.Main;
using Client.Tracing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Client.Desktop;

public static class Program
{
    [STAThread]
    public static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((_, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((_, services) =>
            {
                services.AddSingleton<IConfigReader, ConfigReader>();

                services.AddPresentationServices();
                services.AddTracingServices();
            })
            .Build();
        
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