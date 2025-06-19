using Domain.Events.Settings;
using Domain.Interfaces;
using Service.Server.CQRS.Commands.Entities.Settings;
using Service.Server.Old.Translators.Settings;

namespace Service.Server.Old.Services.Entities.Settings;

public class SettingsCommandsService(
    IEventStore<SettingsEvent> settingsEventStore,
    ISettingsCommandsToEventTranslator eventTranslator)
    : ISettingsCommandsService
{
    public async Task Update(UpdateSettingsCommand updateSettingsCommand)
    {
        await settingsEventStore.StoreEventAsync(
            eventTranslator.ToEvent(updateSettingsCommand));
    }

    public async Task Create(CreateSettingsCommand createSettingsCommand)
    {
        await settingsEventStore.StoreEventAsync(
            eventTranslator.ToEvent(createSettingsCommand));
    }
}