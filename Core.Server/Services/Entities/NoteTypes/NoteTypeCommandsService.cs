
using Domain.Events.NoteTypes;
using Domain.Interfaces;
using Service.Server.Communication.CQRS.Commands.Entities.NoteType;
using Service.Server.Communication.Services.NoteType;
using Service.Server.Translators.NoteTypes;

namespace Service.Server.Services.Entities.NoteTypes;

public class NoteTypeCommandsService(
    NoteTypeNotificationService noteNotificationService,
    IEventStore<NoteTypeEvent> noteTypeEventStore,
    INoteTypeCommandsToEventTranslator eventTranslator) : INoteTypeCommandsService
{
    public async Task Create(CreateNoteTypeCommand createNoteTypeCommand)
    {
        await noteTypeEventStore.StoreEventAsync(eventTranslator.ToEvent(createNoteTypeCommand));
        await noteNotificationService.SendNotificationAsync(createNoteTypeCommand.ToNotification());
    }

    public async Task ChangeName(ChangeNoteTypeNameCommand changeNoteTypeNameCommand)
    {
        await noteTypeEventStore.StoreEventAsync(eventTranslator.ToEvent(changeNoteTypeNameCommand));
        await noteNotificationService.SendNotificationAsync(changeNoteTypeNameCommand.ToNotification());
    }

    public async Task ChangeColor(ChangeNoteTypeColorCommand changeNoteTypeColorCommand)
    {
        await noteTypeEventStore.StoreEventAsync(eventTranslator.ToEvent(changeNoteTypeColorCommand));
        await noteNotificationService.SendNotificationAsync(changeNoteTypeColorCommand.ToNotification());
    }
}