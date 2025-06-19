using System.Text.Json;
using Domain.Events.NoteTypes;
using Service.Server.CQRS.Commands.Entities.NoteType;

namespace Service.Server.Old.Translators.NoteTypes;

public class NoteTypeCommandsToEventTranslator : INoteTypeCommandsToEventTranslator
{
    public NoteTypeEvent ToEvent(CreateNoteTypeCommand command)
    {
        var domainEvent = new NoteTypeCreatedEvent(command.NoteTypeId, command.Name, command.Color);

        return CreateDatabaseEvent(nameof(NoteTypeCreatedEvent), command.NoteTypeId,
            JsonSerializer.Serialize(domainEvent));
    }

    public NoteTypeEvent ToEvent(ChangeNoteTypeNameCommand command)
    {
        var domainEvent = new NoteTypeNameChangedEvent(command.NoteTypeId, command.Name);

        return CreateDatabaseEvent(nameof(NoteTypeNameChangedEvent), command.NoteTypeId,
            JsonSerializer.Serialize(domainEvent));
    }

    public NoteTypeEvent ToEvent(ChangeNoteTypeColorCommand command)
    {
        var domainEvent = new NoteTypeColorChangedEvent(command.NoteTypeId, command.Color);

        return CreateDatabaseEvent(nameof(NoteTypeColorChangedEvent), command.NoteTypeId,
            JsonSerializer.Serialize(domainEvent));
    }

    private static NoteTypeEvent CreateDatabaseEvent(string eventName, Guid entityId, string json)
    {
        return new NoteTypeEvent(Guid.NewGuid(), DateTime.UtcNow, TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow),
            eventName, entityId, json);
    }
}