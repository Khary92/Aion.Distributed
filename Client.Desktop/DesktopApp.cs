using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.ReactiveUI;
using Client.Desktop.Services.Cache;
using Client.Desktop.Services.Initializer;
using Client.Desktop.Views.Main;
using Client.Tracing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Proto.Command.TimeSlots;

namespace Client.Desktop;

public static class DesktopApp
{
    [STAThread]
    public static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddPresentationServices();
                services.AddTracingServices();
            })
            .Build();

        await host.StartAsync();

        var serviceProvider = host.Services;

        await StartupPreparation(serviceProvider);

        BuildAvaloniaApp(serviceProvider)
            .StartWithClassicDesktopLifetime(args);

        await host.StopAsync();
    }


    private static AppBuilder BuildAvaloniaApp(IServiceProvider serviceProvider)
    {
        return AppBuilder.Configure(() => new App(serviceProvider))
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
    }

    private static async Task StartupPreparation(IServiceProvider serviceProvider)
    {
        AppDomain.CurrentDomain.ProcessExit += (_, _) => { PersistCaches(serviceProvider).GetAwaiter().GetResult(); };

        await serviceProvider.GetRequiredService<IServiceInitializer>().InitializeServicesAsync();
        await PersistCaches(serviceProvider);
    }

    private static async Task PersistCaches(IServiceProvider serviceProvider)
    {
        var startTimeCache = serviceProvider.GetRequiredService<IPersistentCache<SetStartTimeCommandProto>>();
        await startTimeCache.Persist();

        var endTimeCache = serviceProvider.GetRequiredService<IPersistentCache<SetEndTimeCommandProto>>();
        await endTimeCache.Persist();
    }
}