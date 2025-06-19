using Domain.Events.NoteTypes;
using Domain.Interfaces;
using Service.Server.CQRS.Commands.Entities.NoteType;
using Service.Server.Old.Translators.NoteTypes;

namespace Service.Server.Old.Services.Entities.NoteTypes;

public class NoteTypeCommandsService(
    IMediator mediator,
    IEventStore<NoteTypeEvent> noteTypeEventStore,
    INoteTypeCommandsToEventTranslator eventTranslator,
    INoteTypeCommandsToNotificationTranslator notificationTranslator) : INoteTypeCommandsService
{
    public async Task Create(CreateNoteTypeCommand createNoteTypeCommand)
    {
        await noteTypeEventStore.StoreEventAsync(eventTranslator.ToEvent(createNoteTypeCommand));
        await mediator.Publish(notificationTranslator.ToNotification(createNoteTypeCommand));
    }

    public async Task ChangeName(ChangeNoteTypeNameCommand changeNoteTypeNameCommand)
    {
        await noteTypeEventStore.StoreEventAsync(eventTranslator.ToEvent(changeNoteTypeNameCommand));
        await mediator.Publish(notificationTranslator.ToNotification(changeNoteTypeNameCommand));
    }

    public async Task ChangeColor(ChangeNoteTypeColorCommand changeNoteTypeColorCommand)
    {
        await noteTypeEventStore.StoreEventAsync(eventTranslator.ToEvent(changeNoteTypeColorCommand));
        await mediator.Publish(notificationTranslator.ToNotification(changeNoteTypeColorCommand));
    }
}