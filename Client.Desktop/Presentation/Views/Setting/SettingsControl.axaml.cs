using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using Client.Desktop.Presentation.Models.Settings;
using Client.Desktop.Services.LocalSettings;

namespace Client.Desktop.Presentation.Views.Setting;

public partial class SettingsControl : ReactiveUserControl<SettingsViewModel>
{
    private readonly ILocalSettingsService? _localSettingsService;

    public SettingsControl()
    {
        InitializeComponent();
    }

    public SettingsControl(SettingsViewModel viewModel, ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
        InitializeComponent();
        DataContext = viewModel;
    }

    private async void OnExportPathClick(object? sender, RoutedEventArgs e)
    {
        _ = HandleExportPathClick();
    }

    private async Task HandleExportPathClick()
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel?.StorageProvider is null)
            return;

        var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Choose folder",
            AllowMultiple = false
        });

        var storageFolder = folders.FirstOrDefault();

        if (storageFolder == null) return;

        await _localSettingsService.SetExportPath(storageFolder.Path.AbsolutePath);
    }
}