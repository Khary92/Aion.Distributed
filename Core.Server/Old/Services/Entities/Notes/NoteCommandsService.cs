using Domain.Events.Note;
using Domain.Interfaces;
using Service.Server.CQRS.Commands.Entities.Note;
using Service.Server.Old.Translators.Notes;

namespace Service.Server.Old.Services.Entities.Notes;

public class NoteCommandsService(
    IMediator mediator,
    IEventStore<NoteEvent> noteEventsStore,
    INoteCommandsToEventTranslator eventTranslator,
    INoteCommandsToNotificationTranslator notificationTranslator) : INoteCommandsService
{
    public async Task Update(UpdateNoteCommand updateNoteCommand)
    {
        await noteEventsStore.StoreEventAsync(eventTranslator.ToEvent(updateNoteCommand));

        await mediator.Publish(notificationTranslator.ToNotification(updateNoteCommand));
    }

    public async Task Create(CreateNoteCommand createNoteCommand)
    {
        await noteEventsStore.StoreEventAsync(eventTranslator.ToEvent(createNoteCommand));

        await mediator.Publish(notificationTranslator.ToNotification(createNoteCommand));
    }
}