using Core.Server.Communication.Endpoints.AiSettings;
using Core.Server.Communication.Records.Commands.Entities.AiSettings;
using Core.Server.Translators.Commands.AiSettings;
using Domain.Events.AiSettings;
using Domain.Interfaces;

namespace Core.Server.Services.Entities.AiSettings;

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