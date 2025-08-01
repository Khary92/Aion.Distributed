using Core.Server.Communication.Endpoints.Tag;
using Core.Server.Communication.Records.Commands.Entities.Tags;
using Core.Server.Translators.Commands.Tags;
using Domain.Events.Tags;
using Domain.Interfaces;

namespace Core.Server.Services.Entities.Tags;

public class TagCommandsService(
    IEventStore<TagEvent> tagEventStore,
    TagNotificationService tagNotificationService,
    ITagCommandsToEventTranslator eventTranslator) : ITagCommandsService
{
    public async Task Update(UpdateTagCommand command)
    {
        await tagEventStore.StoreEventAsync(eventTranslator.ToEvent(command), command.TraceId);
        await tagNotificationService.SendNotificationAsync(command.ToNotification());
    }

    public async Task Create(CreateTagCommand command)
    {
        await tagEventStore.StoreEventAsync(eventTranslator.ToEvent(command), command.TraceId);
        await tagNotificationService.SendNotificationAsync(command.ToNotification());
    }
}