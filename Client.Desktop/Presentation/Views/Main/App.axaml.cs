using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Client.Desktop.Communication.Notifications;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Desktop.Presentation.Views.Main;

public class App : Application
{
    private readonly IServiceProvider _serviceProvider;

    public App(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (!Design.IsDesignMode)
        {
            var contentWrapper = _serviceProvider.GetRequiredService<ContentWrapper>();
            contentWrapper.WindowState = WindowState.Maximized;
            contentWrapper.Show();

            var notifiationService = _serviceProvider.GetRequiredService<NotificationReceiverStarter>();
            _ = notifiationService.Start();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void TrayIconMaximize_OnClick(object? sender, EventArgs e)
    {
        var contentWrapper = _serviceProvider.GetRequiredService<ContentWrapper>();
        contentWrapper.Show();
        contentWrapper.WindowState = WindowState.Maximized;
    }

    private void TrayIconClose_OnClick(object? sender, EventArgs e)
    {
        if (Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopApp) desktopApp.Shutdown();
    }
}