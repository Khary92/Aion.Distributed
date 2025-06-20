using Domain.Events.Tags;
using Domain.Interfaces;
using Service.Server.Communication.CQRS.Commands.Entities.Tags;
using Service.Server.Communication.Services.Tag;
using Service.Server.Translators.Tags;

namespace Service.Server.Services.Entities.Tags;

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