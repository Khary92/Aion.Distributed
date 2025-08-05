using Core.Server.Communication.Endpoints.Note;
using Core.Server.Communication.Records.Commands.Entities.Note;
using Core.Server.Tracing.Tracing.Tracers;
using Core.Server.Translators.Commands.Notes;
using Domain.Events.Note;
using Domain.Interfaces;

namespace Core.Server.Services.Entities.Notes;

public class NoteCommandsService(
    NoteNotificationService noteNotificationService,
    IEventStore<NoteEvent> noteEventsStore,
    INoteCommandsToEventTranslator eventTranslator,
    ITraceCollector tracer) : INoteCommandsService
{
    public async Task Update(UpdateNoteCommand command)
    {
        await noteEventsStore.StoreEventAsync(eventTranslator.ToEvent(command), command.TraceId);
        var noteNotification = command.ToNotification();
        await tracer.Note.Update.EventPersisted(GetType(), command.TraceId, noteNotification.NoteUpdated);

        await tracer.Note.Update.SendingNotification(GetType(), command.TraceId, noteNotification.NoteUpdated);
        await noteNotificationService.SendNotificationAsync(command.ToNotification());
    }

    public async Task Create(CreateNoteCommand command)
    {
        await noteEventsStore.StoreEventAsync(eventTranslator.ToEvent(command), command.TraceId);
        var noteNotification = command.ToNotification();
        await tracer.Note.Create.EventPersisted(GetType(), command.TraceId, noteNotification.NoteCreated);

        await tracer.Note.Create.SendingNotification(GetType(), command.TraceId, noteNotification.NoteCreated);
        await noteNotificationService.SendNotificationAsync(command.ToNotification());
    }
}