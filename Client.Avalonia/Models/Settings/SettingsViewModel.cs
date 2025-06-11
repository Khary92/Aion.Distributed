using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Client.Avalonia.ViewModels.Settings;

public class SettingsViewModel : ReactiveObject
{
    public SettingsViewModel(SettingsModel settingsModel)
    {
        Model = settingsModel;

        SaveConfigCommand = ReactiveCommand.CreateFromTask(Model.SaveConfigAsync);

        Model.InitializeAsync().ConfigureAwait(false);
    }

    public SettingsModel Model { get; }

    public ReactiveCommand<Unit, Unit> SaveConfigCommand { get; }
}