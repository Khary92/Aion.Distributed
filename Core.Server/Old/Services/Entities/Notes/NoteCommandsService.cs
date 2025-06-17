using Application.Contract.CQRS.Commands.Entities.Note;
using Application.Translators.Notes;
using Domain.Events.Note;
using Domain.Interfaces;
using MediatR;

namespace Application.Services.Entities.Notes;

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