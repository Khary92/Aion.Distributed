using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform;
using Client.Desktop.Models.Main;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;

namespace Client.Desktop.Views.Main;

public partial class ContentWrapper : Window
{
    public ContentWrapper(ContentWrapperViewModel viewModel)
    {
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
            Avalonia.Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopApp)
            desktopApp.Shutdown();
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