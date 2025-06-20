using Domain.Events.AiSettings;
using Domain.Interfaces;
using Service.Server.Communication.AiSettings;
using Service.Server.CQRS.Commands.Entities.AiSettings;
using Service.Server.Old.Translators.AiSettings;

namespace Service.Server.Old.Services.Entities.AiSettings;

public class AiSettingsCommandsService(
    IEventStore<AiSettingsEvent> aiSettingsEventsStore,
    IAiSettingsCommandsToEventTranslator eventTranslator, 
    AiSettingsNotificationService aiSettingsNotificationService)
    : IAiSettingsCommandsService
{
    public async Task Create(CreateAiSettingsCommand @event)
    {
        await aiSettingsEventsStore.StoreEventAsync(eventTranslator.ToEvent(@event));
        await aiSettingsNotificationService.SendNotificationAsync(@event.ToNotification());
    }

    public async Task ChangePrompt(ChangePromptCommand @event)
    {
        await aiSettingsEventsStore.StoreEventAsync(eventTranslator.ToEvent(@event));
        await aiSettingsNotificationService.SendNotificationAsync(@event.ToNotification());
    }

    public async Task ChangeLanguageModelPath(ChangeLanguageModelCommand @event)
    {
        await aiSettingsEventsStore.StoreEventAsync(eventTranslator.ToEvent(@event));
        await aiSettingsNotificationService.SendNotificationAsync(@event.ToNotification());
    }
}