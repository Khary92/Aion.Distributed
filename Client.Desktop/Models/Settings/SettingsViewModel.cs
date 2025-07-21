using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Client.Desktop.Models.Settings;

public class SettingsViewModel : ReactiveObject
{
    private string _exportPathInput;

    public string ExportPathInput
    {
        get => _exportPathInput;
        private set => this.RaiseAndSetIfChanged(ref _exportPathInput, value);
    }

    public SettingsViewModel(SettingsModel settingsModel)
    {
        Model = settingsModel;
        _exportPathInput = settingsModel.Settings.ExportPath;
        SaveConfigCommand = ReactiveCommand.CreateFromTask(() => Model.SetExportPath(ExportPathInput));
    }

    public SettingsModel Model { get; }
    
    public ReactiveCommand<Unit, Unit> SaveConfigCommand { get; }
}