using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.ReactiveUI;
using Client.Avalonia.Communication.Notifications.Notes;
using Client.Avalonia.Communication.RequiresChange.Cache;
using Client.Avalonia.Views.Main;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Proto.Command.TimeSlots;

namespace Client.Avalonia;

public static class DesktopApp
{
    [STAThread]
    public static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddPresentationServices();
            })
            .Build();

        await host.StartAsync();

        var serviceProvider = host.Services;

        await StartupPreparation(serviceProvider);

        BuildAvaloniaApp(serviceProvider)
            .StartWithClassicDesktopLifetime(args);

        await host.StopAsync();
    }

    private static async Task StartupPreparation(IServiceProvider serviceProvider)
    {
        AppDomain.CurrentDomain.ProcessExit += (_, _) =>
        {
            PersistCaches(serviceProvider).GetAwaiter().GetResult();
        };

        await PersistCaches(serviceProvider);
    }

    private static async Task PersistCaches(IServiceProvider serviceProvider)
    {
        var startTimeCache = serviceProvider.GetRequiredService<IPersistentCache<SetStartTimeCommand>>();
        await startTimeCache.Persist();

        var endTimeCache = serviceProvider.GetRequiredService<IPersistentCache<SetEndTimeCommand>>();
        await endTimeCache.Persist();
    }

    private static AppBuilder BuildAvaloniaApp(IServiceProvider serviceProvider)
    {
        return AppBuilder.Configure(() => new App(serviceProvider))
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
    }
}