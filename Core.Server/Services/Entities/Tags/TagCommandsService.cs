
using Core.Server.Communication.Endpoints.Tag;
using Core.Server.Communication.Records.Commands.Entities.Tags;
using Core.Server.Tracing.Tracing.Tracers;
using Core.Server.Translators.Commands.Tags;
using Domain.Events.Tags;
using Domain.Interfaces;

namespace Core.Server.Services.Entities.Tags;

public class TagCommandsService(
    IEventStore<TagEvent> tagEventStore,
    TagNotificationService tagNotificationService,
    ITagCommandsToEventTranslator eventTranslator,
    ITraceCollector tracer) : ITagCommandsService
{
    public async Task Update(UpdateTagCommand command)
    {
        await tagEventStore.StoreEventAsync(eventTranslator.ToEvent(command));
        var noteNotification = command.ToNotification();
        await tracer.Tag.Update.EventPersisted(GetType(), command.TraceId, noteNotification.TagUpdated);
        
        await tracer.Tag.Update.SendingNotification(GetType(), command.TraceId, noteNotification.TagUpdated);
        await tagNotificationService.SendNotificationAsync(command.ToNotification());
    }

    public async Task Create(CreateTagCommand command)
    {
        await tagEventStore.StoreEventAsync(eventTranslator.ToEvent(command));
        var noteNotification = command.ToNotification();
        await tracer.Tag.Create.EventPersisted(GetType(), command.TraceId, noteNotification.TagCreated);

        await tracer.Tag.Create.SendingNotification(GetType(), command.TraceId, noteNotification.TagCreated);
        await tagNotificationService.SendNotificationAsync(command.ToNotification());
    }
}