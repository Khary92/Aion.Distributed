using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using Client.Avalonia.Models.Settings;

namespace Client.Avalonia.Views.Setting;

public partial class AiSettingsControl : ReactiveUserControl<AiSettingsViewModel>
{
    public AiSettingsControl(AiSettingsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    public AiSettingsControl()
    {
        InitializeComponent();
    }

    private async void LanguageModelPathClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel?.StorageProvider is null)
            return;

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Choose language model gguf file",
            AllowMultiple = false,
            FileTypeFilter =
            [
                new FilePickerFileType("GGUF Files")
                {
                    Patterns = ["*.gguf"]
                }
            ]
        });

        var selectedFile = files.FirstOrDefault();

        if (selectedFile == null) return;

        ViewModel!.Model.AiSettings!.LanguageModelPath = selectedFile.Path.AbsolutePath;
    }
}