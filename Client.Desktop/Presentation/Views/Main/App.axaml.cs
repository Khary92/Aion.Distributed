using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Client.Desktop.Lifecycle.Shutdown;
using Client.Desktop.Lifecycle.Startup.Scheduler;
using Client.Desktop.Presentation.Views.Mock;
using Client.Desktop.Services.Authentication;
using Client.Desktop.Services.Mock;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Desktop.Presentation.Views.Main;

public class App(IServiceProvider serviceProvider) : Application
{
    public override void OnFrameworkInitializationCompleted()
    {
        var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.WindowState = WindowState.Maximized;
        mainWindow.Show();

        var mockSettingsService = serviceProvider.GetRequiredService<IMockSettingsService>();
        if (mockSettingsService.IsMockingModeActive)
        {
            var debugWindow = serviceProvider.GetRequiredService<DebugWindow>();
            debugWindow.Show();
        }

        _ = serviceProvider.GetRequiredService<IEventRegistration>().Execute();

        var tokenService = serviceProvider.GetRequiredService<ITokenService>();
        tokenService.Authenticated += LoadData;

        base.OnFrameworkInitializationCompleted();
    }

    private async Task LoadData(string token)
    {
        var startupScheduler = serviceProvider.GetRequiredService<IStartupScheduler>();
        await startupScheduler.Execute();
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void TrayIconMaximize_OnClick(object? sender, EventArgs e)
    {
        var contentWrapper = serviceProvider.GetRequiredService<MainWindow>();
        contentWrapper.Show();
        contentWrapper.WindowState = WindowState.Maximized;
    }

    private void TrayIconClose_OnClick(object? sender, EventArgs e)
    {
        var shutdownHandler = serviceProvider.GetRequiredService<IShutDownHandler>();
        shutdownHandler.Exit();
    }
}