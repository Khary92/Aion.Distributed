using Core.Server.Communication.Endpoints.NoteType;
using Core.Server.Communication.Records.Commands.Entities.NoteType;
using Core.Server.Tracing.Tracing.Tracers;
using Core.Server.Translators.Commands.NoteTypes;
using Domain.Events.NoteTypes;
using Domain.Interfaces;

namespace Core.Server.Services.Entities.NoteTypes;

public class NoteTypeCommandsService(
    NoteTypeNotificationService noteTypeNotificationService,
    IEventStore<NoteTypeEvent> noteTypeEventStore,
    INoteTypeCommandsToEventTranslator eventTranslator,
    ITraceCollector tracer) : INoteTypeCommandsService
{
    public async Task Create(CreateNoteTypeCommand command)
    {
        await noteTypeEventStore.StoreEventAsync(eventTranslator.ToEvent(command));

        var notification = command.ToNotification();
        await tracer.NoteType.Create.EventPersisted(GetType(), command.TraceId, notification);

        await tracer.NoteType.Create.SendingNotification(GetType(), command.TraceId, notification);
        await noteTypeNotificationService.SendNotificationAsync(notification);
    }

    public async Task ChangeName(ChangeNoteTypeNameCommand command)
    {
        await noteTypeEventStore.StoreEventAsync(eventTranslator.ToEvent(command));

        var notification = command.ToNotification();
        await tracer.NoteType.ChangeName.EventPersisted(GetType(), command.TraceId, notification);

        await tracer.NoteType.ChangeName.SendingNotification(GetType(), command.TraceId, notification);
        await noteTypeNotificationService.SendNotificationAsync(notification);
    }

    public async Task ChangeColor(ChangeNoteTypeColorCommand command)
    {
        await noteTypeEventStore.StoreEventAsync(eventTranslator.ToEvent(command));
        var notification = command.ToNotification();
        
        await tracer.NoteType.ChangeColor.EventPersisted(GetType(), command.TraceId,
            notification.NoteTypeColorChanged);

        await tracer.NoteType.ChangeColor.SendingNotification(GetType(), command.TraceId,
            notification.NoteTypeColorChanged);
        await noteTypeNotificationService.SendNotificationAsync(command.ToNotification());
    }
}