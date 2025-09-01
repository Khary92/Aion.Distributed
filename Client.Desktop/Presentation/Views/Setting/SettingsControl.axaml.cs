using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using Client.Desktop.Presentation.Models.Settings;

namespace Client.Desktop.Presentation.Views.Setting;

public partial class SettingsControl : ReactiveUserControl<SettingsViewModel>
{
    public SettingsControl()
    {
        InitializeComponent();
    }

    public SettingsControl(SettingsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    private async void OnExportPathClick(object? sender, RoutedEventArgs e)
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

        ViewModel!.Model.Settings!.ExportPath = storageFolder.Path.AbsolutePath;
    }
}