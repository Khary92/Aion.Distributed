using Client.Desktop.DataModels.Local;
using Client.Desktop.Lifecycle.Startup.Tasks.Register;
using Client.Desktop.Services.LocalSettings;
using Client.Desktop.Services.LocalSettings.Commands;
using CommunityToolkit.Mvvm.Messaging;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.Settings;

public class SettingsModel(IMessenger messenger, ILocalSettingsCommandSender localSettingsCommandService)
    : ReactiveObject, IRegisterMessenger
{
    private SettingsClientModel? _settingsDto;

    public SettingsClientModel? Settings
    {
        get => _settingsDto;
        private set => this.RaiseAndSetIfChanged(ref _settingsDto, value);
    }

    public void RegisterMessenger()
    {
        messenger.Register<ExportPathSetNotification>(this, void (_, m) => { Settings!.ExportPath = m.ExportPath; });

        messenger.Register<SettingsClientModel>(this, void (_, m) => { Settings = m; });
        
        messenger.Register<WorkDaySelectedNotification>(this, void (_, m) =>
        {
            if (Settings == null) return;
            
            Settings.SelectedDate = m.Date;
        });
    }

    public void SetExportPath()
    {
        localSettingsCommandService.Send(new SetExportPathCommand(Settings!.ExportPath));
    }
}