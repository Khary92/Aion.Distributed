using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Client.Desktop.Lifecycle.Shutdown;
using Client.Desktop.Lifecycle.Startup.Scheduler;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Desktop.Presentation.Views.Main;

public class App(IServiceProvider serviceProvider) : Application
{
    public override void OnFrameworkInitializationCompleted()
    {
        var contentWrapper = serviceProvider.GetRequiredService<ContentWrapper>();
        contentWrapper.WindowState = WindowState.Maximized;
        contentWrapper.Show();

        _ = serviceProvider.GetRequiredService<IStartupScheduler>().Execute();

        base.OnFrameworkInitializationCompleted();
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void TrayIconMaximize_OnClick(object? sender, EventArgs e)
    {
        var contentWrapper = serviceProvider.GetRequiredService<ContentWrapper>();
        contentWrapper.Show();
        contentWrapper.WindowState = WindowState.Maximized;
    }

    private void TrayIconClose_OnClick(object? sender, EventArgs e)
    {
        if (Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktopApp) return;

        var shutdownHandler = serviceProvider.GetRequiredService<IShutDownHandler>();
        shutdownHandler.Exit(desktopApp);
    }
}