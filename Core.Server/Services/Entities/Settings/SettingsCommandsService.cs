using Domain.Events.Settings;
using Domain.Interfaces;
using Service.Server.Communication.CQRS.Commands.Entities.Settings;
using Service.Server.Communication.Services.Settings;
using Service.Server.Translators.Settings;

namespace Service.Server.Services.Entities.Settings;

public class SettingsCommandsService(
    SettingsNotificationService settingsNotificationService,
    IEventStore<SettingsEvent> settingsEventStore,
    ISettingsCommandsToEventTranslator eventTranslator)
    : ISettingsCommandsService
{
    public Task ChangeAutomaticTicketAddingToSprint(ChangeAutomaticTicketAddingToSprintCommand command)
    {
        throw new NotImplementedException();
    }

    public Task ChangeExportPath(ChangeExportPathCommand command)
    {
        throw new NotImplementedException();
    }

    public async Task Create(CreateSettingsCommand createSettingsCommand)
    {
        await settingsEventStore.StoreEventAsync(
            eventTranslator.ToEvent(createSettingsCommand));
        
        await settingsNotificationService.SendNotificationAsync(createSettingsCommand.ToNotification());
    }
}