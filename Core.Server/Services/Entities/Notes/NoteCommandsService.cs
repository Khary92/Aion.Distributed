using Domain.Events.Note;
using Domain.Interfaces;
using Service.Server.Communication.CQRS.Commands.Entities.Note;
using Service.Server.Communication.Services.Note;
using Service.Server.Translators.Notes;

namespace Service.Server.Services.Entities.Notes;

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