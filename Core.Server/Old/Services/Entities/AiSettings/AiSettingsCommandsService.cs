using Application.Contract.CQRS.Commands.Entities.AiSettings;
using Application.Translators.AiSettings;
using Domain.Events.AiSettings;
using Domain.Interfaces;

namespace Application.Services.Entities.AiSettings;

public class AiSettingsCommandsService(
    IEventStore<AiSettingsEvent> aiSettingsEventsStore,
    IAiSettingsCommandsToEventTranslator eventTranslator)
    : IAiSettingsCommandsService
{
    public async Task Create(CreateAiSettingsCommand @event)
    {
        await aiSettingsEventsStore.StoreEventAsync(eventTranslator.ToEvent(@event));
    }

    public async Task ChangePrompt(ChangePromptCommand @event)
    {
        await aiSettingsEventsStore.StoreEventAsync(eventTranslator.ToEvent(@event));
    }

    public async Task ChangeLanguageModelPath(ChangeLanguageModelCommand @event)
    {
        await aiSettingsEventsStore.StoreEventAsync(eventTranslator.ToEvent(@event));
    }
}