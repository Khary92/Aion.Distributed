using Domain.Events.Settings;
using Domain.Interfaces;
using Service.Server.Communication.Settings;
using Service.Server.CQRS.Commands.Entities.Settings;
using Service.Server.Old.Translators.Settings;

namespace Service.Server.Old.Services.Entities.Settings;

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