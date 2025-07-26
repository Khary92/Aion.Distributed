using Client.Desktop.DTO.Local;
using Client.Desktop.Services.Initializer;
using Client.Desktop.Services.LocalSettings;
using Client.Desktop.Services.LocalSettings.Commands;
using CommunityToolkit.Mvvm.Messaging;
using ReactiveUI;

namespace Client.Desktop.Models.Settings;

public class SettingsModel(IMessenger messenger, ILocalSettingsCommandSender localSettingsCommandService)
    : ReactiveObject, IRegisterMessenger
{
    private SettingsDto? _settingsDto;

    public SettingsDto? Settings
    {
        get => _settingsDto;
        private set => this.RaiseAndSetIfChanged(ref _settingsDto, value);
    }

    public void RegisterMessenger()
    {
        messenger.Register<ExportPathSetNotification>(this,
            async void (_, m) => { Settings!.ExportPath = m.ExportPath; });

        messenger.Register<SettingsDto>(this, async void (_, m) => { Settings = m; });
    }

    public void SetExportPath()
    {
        localSettingsCommandService.Send(new SetExportPathCommand(Settings!.ExportPath));
        ;
    }
}