using Client.Desktop.DataModels.Local;
using Client.Desktop.Lifecycle.Startup.Tasks.Register;
using Client.Desktop.Services.LocalSettings;
using Client.Desktop.Services.LocalSettings.Commands;
using CommunityToolkit.Mvvm.Messaging;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.Settings;

public class SettingsModel(IMessenger messenger, ILocalSettingsCommandSender localSettingsCommandService)
    : ReactiveObject, IMessengerRegistration, IRecipient<ExportPathSetNotification>,
        IRecipient<WorkDaySelectedNotification>, IRecipient<SettingsClientModel>
{
    private SettingsClientModel? _settingsDto;

    public SettingsClientModel? Settings
    {
        get => _settingsDto;
        private set => this.RaiseAndSetIfChanged(ref _settingsDto, value);
    }

    public void RegisterMessenger()
    {
        messenger.RegisterAll(this);
    }

    public void UnregisterMessenger()
    {
        messenger.UnregisterAll(this);
    }

    public void Receive(ExportPathSetNotification message)
    {
        Settings!.ExportPath = message.ExportPath;
    }

    public void Receive(SettingsClientModel message)
    {
        Settings = message;
    }

    public void Receive(WorkDaySelectedNotification message)
    {
        if (Settings == null) return;

        Settings.SelectedDate = message.Date;
    }

    public void SetExportPath()
    {
        localSettingsCommandService.Send(new SetExportPathCommand(Settings!.ExportPath));
    }
}