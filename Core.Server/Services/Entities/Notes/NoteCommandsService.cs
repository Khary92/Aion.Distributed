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
    public async Task Update(UpdateNoteCommand updateNoteCommand)
    {
        await noteEventsStore.StoreEventAsync(eventTranslator.ToEvent(updateNoteCommand));
        await noteNotificationService.SendNotificationAsync(updateNoteCommand.ToNotification());
    }

    public async Task Create(CreateNoteCommand createNoteCommand)
    {
        await noteEventsStore.StoreEventAsync(eventTranslator.ToEvent(createNoteCommand));
        await noteNotificationService.SendNotificationAsync(createNoteCommand.ToNotification());
    }
}