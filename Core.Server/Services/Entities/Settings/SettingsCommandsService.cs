using Core.Server.Communication.Endpoints.Settings;
using Core.Server.Communication.Records.Commands.Entities.Settings;
using Core.Server.Translators.Commands.Settings;
using Domain.Events.Settings;
using Domain.Interfaces;

namespace Core.Server.Services.Entities.Settings;

public class SettingsCommandsService(
    SettingsNotificationService settingsNotificationService,
    IEventStore<SettingsEvent> settingsEventStore,
    ISettingsCommandsToEventTranslator eventTranslator)
    : ISettingsCommandsService
{
    public async Task ChangeAutomaticTicketAddingToSprint(ChangeAutomaticTicketAddingToSprintCommand command)
    {
        await settingsEventStore.StoreEventAsync(eventTranslator.ToEvent(command));
        await settingsNotificationService.SendNotificationAsync(command.ToNotification());
    }

    public async Task ChangeExportPath(ChangeExportPathCommand command)
    {
        await settingsEventStore.StoreEventAsync(eventTranslator.ToEvent(command));
        await settingsNotificationService.SendNotificationAsync(command.ToNotification());
    }

    public async Task Create(CreateSettingsCommand command)
    {
        await settingsEventStore.StoreEventAsync(eventTranslator.ToEvent(command));
        await settingsNotificationService.SendNotificationAsync(command.ToNotification());
    }
}