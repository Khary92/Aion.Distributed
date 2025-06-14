using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Client.Desktop.Models.Settings;

public class SettingsViewModel : ReactiveObject
{
    public SettingsViewModel(SettingsModel settingsModel)
    {
        Model = settingsModel;

        SaveConfigCommand = ReactiveCommand.CreateFromTask(Model.SaveConfigAsync);
        
        Model.InitializeAsync().ConfigureAwait(false);
        Model.RegisterMessenger();
    }

    public SettingsModel Model { get; }

    public ReactiveCommand<Unit, Unit> SaveConfigCommand { get; }
}