using Core.Server.Communication.Endpoints.Note;
using Core.Server.Communication.Records.Commands.Entities.Note;
using Core.Server.Translators.Commands.Notes;
using Domain.Events.Note;
using Domain.Interfaces;

namespace Core.Server.Services.Entities.Notes;

public class NoteCommandsService(
    NoteNotificationService noteNotificationService,
    IEventStore<NoteEvent> noteEventsStore,
    INoteCommandsToEventTranslator eventTranslator) : INoteCommandsService
{
    public async Task Update(UpdateNoteCommand command)
    {
        await noteEventsStore.StoreEventAsync(eventTranslator.ToEvent(command), command.TraceId);
        await noteNotificationService.SendNotificationAsync(command.ToNotification());
    }

    public async Task Create(CreateNoteCommand command)
    {
        await noteEventsStore.StoreEventAsync(eventTranslator.ToEvent(command), command.TraceId);
        await noteNotificationService.SendNotificationAsync(command.ToNotification());
    }
}