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
    public async Task Create(CreateNoteTypeCommand command)
    {
        await noteTypeEventStore.StoreEventAsync(eventTranslator.ToEvent(command), command.TraceId);
        await noteNotificationService.SendNotificationAsync(command.ToNotification());
    }

    public async Task ChangeName(ChangeNoteTypeNameCommand command)
    {
        await noteTypeEventStore.StoreEventAsync(eventTranslator.ToEvent(command), command.TraceId);
        await noteNotificationService.SendNotificationAsync(command.ToNotification());
    }

    public async Task ChangeColor(ChangeNoteTypeColorCommand command)
    {
        await noteTypeEventStore.StoreEventAsync(eventTranslator.ToEvent(command), command.TraceId);
        await noteNotificationService.SendNotificationAsync(command.ToNotification());
    }
}