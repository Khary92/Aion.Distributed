using Application.Contract.CQRS.Commands.Entities.Settings;
using Application.Translators.Settings;
using Domain.Events.Settings;
using Domain.Interfaces;

namespace Application.Services.Entities.Settings;

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