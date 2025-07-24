using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Client.Desktop.Models.Settings;

public class SettingsViewModel : ReactiveObject
{
    public SettingsViewModel(SettingsModel settingsModel)
    {
        Model = settingsModel;
        SaveConfigCommand = ReactiveCommand.Create(() => Model.SetExportPath());
    }

    public SettingsModel Model { get; }
    public ReactiveCommand<Unit, Unit>? SaveConfigCommand { get; internal set; }
}