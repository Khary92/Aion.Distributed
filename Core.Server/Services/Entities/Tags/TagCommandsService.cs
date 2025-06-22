using Core.Server.Communication.Endpoints.Tag;
using Core.Server.Communication.Records.Commands.Entities.Tags;
using Core.Server.Translators.Commands.Tags;
using Domain.Events.Tags;
using Domain.Interfaces;

namespace Core.Server.Services.Entities.Tags;

public class TagCommandsService(
    IEventStore<TagEvent> tagEventStore,
    TagNotificationServiceImpl tagNotificationService,
    ITagCommandsToEventTranslator eventTranslator) : ITagCommandsService
{
    public async Task Update(UpdateTagCommand updateTagCommand)
    {
        await tagEventStore.StoreEventAsync(eventTranslator.ToEvent(updateTagCommand));
        await tagNotificationService.SendNotificationAsync(updateTagCommand.ToNotification());
    }

    public async Task Create(CreateTagCommand createTagCommand)
    {
        await tagEventStore.StoreEventAsync(eventTranslator.ToEvent(createTagCommand));
        await tagNotificationService.SendNotificationAsync(createTagCommand.ToNotification());
    }
}