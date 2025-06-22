using Core.Server.Communication.Endpoints.NoteType;
using Core.Server.Communication.Records.Commands.Entities.NoteType;
using Core.Server.Translators.Commands.NoteTypes;
using Domain.Events.NoteTypes;
using Domain.Interfaces;

namespace Core.Server.Services.Entities.NoteTypes;

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