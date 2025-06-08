using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;

namespace Client.Avalonia.ViewModels.Settings;

public class TimerSettingsViewModel : ReactiveObject
{
    public TimerSettingsViewModel(TimerSettingsModel settingsModel)
    {
        Model = settingsModel;

        SaveSettingsCommand = ReactiveCommand.CreateFromTask(SaveSettings);

        Model.RegisterMessenger();
        Model.Initialize().ConfigureAwait(false);
    }

    public TimerSettingsModel Model { get; }

    public ReactiveCommand<Unit, Unit> SaveSettingsCommand { get; }

    private async Task SaveSettings()
    {
        await Model.Save();
    }
}