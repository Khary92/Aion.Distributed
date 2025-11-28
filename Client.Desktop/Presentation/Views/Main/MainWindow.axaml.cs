using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform;
using Client.Desktop.Lifecycle.Shutdown;
using Client.Desktop.Presentation.Models.Main;
using Microsoft.Extensions.Hosting;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;

namespace Client.Desktop.Presentation.Views.Main;

public partial class MainWindow : Window
{
    private readonly IHost _host;
    private readonly IShutDownHandler _shutDownHandler;

    public MainWindow()
    {
        InitializeComponent();
        _shutDownHandler = null!;
        _host = null!;
    }

    public MainWindow(MainWindowViewModel viewModel, IShutDownHandler shutDownHandler, IHost host)
    {
        _shutDownHandler = shutDownHandler;
        _host = host;
        InitializeComponent();
        DataContext = viewModel;

        TransparencyLevelHint =
        [
            WindowTransparencyLevel.AcrylicBlur, WindowTransparencyLevel.Blur, WindowTransparencyLevel.Transparent
        ];

        ExtendClientAreaToDecorationsHint = true;
        ExtendClientAreaChromeHints = ExtendClientAreaChromeHints.NoChrome;
        ExtendClientAreaTitleBarHeightHint = -1;
    }

    private void TitleBar_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        BeginMoveDrag(e);
    }

    private async void CloseApp(object? sender, RoutedEventArgs e)
    {
        var messageBoxStandard = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
        {
            ButtonDefinitions = ButtonEnum.YesNo,
            Icon = MsBox.Avalonia.Enums.Icon.Question,
            ContentTitle = "Close Application",
            ContentMessage = "Tracking will be stopped.\nAre you sure you want to close?"
        });

        var result = await messageBoxStandard.ShowAsync();

        if (result == ButtonResult.Yes &&
            Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopApp)
        {
            await _shutDownHandler.Exit();
            await _host.StopAsync();
            desktopApp.Shutdown();
        }
    }

    private void MinimizeApp(object? sender, RoutedEventArgs e)
    {
        Hide();
    }

    private void MaximizeApp(object? sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Maximized;
    }
}