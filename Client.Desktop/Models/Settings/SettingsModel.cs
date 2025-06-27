using System;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.NotificationWrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.DTO;
using CommunityToolkit.Mvvm.Messaging;
using Proto.Command.Settings;
using Proto.Notifications.Settings;
using Proto.Requests.Settings;
using ReactiveUI;

namespace Client.Desktop.Models.Settings;

public class SettingsModel(ICommandSender commandSender, IRequestSender requestSender, IMessenger messenger)
    : ReactiveObject
{
    private SettingsDto _settingsDto = new(Guid.Empty, "settings not loaded from server", false);

    public SettingsDto Settings
    {
        get => _settingsDto;
        private set => this.RaiseAndSetIfChanged(ref _settingsDto, value);
    }

    public async Task InitializeAsync()
    {
        if (await requestSender.Send(new SettingsExistsRequestProto()))
        {
            Settings = await requestSender.Send(new GetSettingsRequestProto());
            return;
        }

        await commandSender.Send(new CreateSettingsCommandProto
        {
            SettingsId = Guid.NewGuid().ToString(),
            ExportPath = "not set",
            IsAddNewTicketsToCurrentSprintActive = false
        });
    }

    public void RegisterMessenger()
    {
        messenger.Register<NewSettingsMessage>(this, (_, m) => { Settings = m.Settings; });

        messenger.Register<ExportPathChangedNotification>(this, (_, m) => { Settings.Apply(m); });

        messenger.Register<AutomaticTicketAddingToSprintChangedNotification>(this, (_, m) => { Settings.Apply(m); });
    }

    public async Task SaveConfigAsync()
    {
        if (Settings.IsExportPathChanged())
            await commandSender.Send(new ChangeExportPathCommandProto
            {
                SettingsId = Settings.SettingsId.ToString(),
                ExportPath = Settings.ExportPath
            });

        if (Settings.IsAddNewTicketsToCurrentSprintChanged())
            await commandSender.Send(new ChangeAutomaticTicketAddingToSprintCommandProto
            {
                SettingsId = Settings.SettingsId.ToString(),
                IsAddNewTicketsToCurrentSprintActive = Settings.IsAddNewTicketsToCurrentSprintActive
            });
    }
}