using System.Threading.Tasks;
using Client.Desktop.Services.Initializer;
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
    
    public ReactiveCommand<Unit, Unit> SaveConfigCommand { get; }
    public InitializationType Type => InitializationType.ViewModel;
    public Task InitializeAsync()
    {
        throw new System.NotImplementedException();
    }
}