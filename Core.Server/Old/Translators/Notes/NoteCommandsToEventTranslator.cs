using System.Text.Json;
using Domain.Events.Note;
using Service.Server.CQRS.Commands.Entities.Note;

namespace Service.Server.Old.Translators.Notes;

public class NoteCommandsToEventTranslator : INoteCommandsToEventTranslator
{
    public NoteEvent ToEvent(CreateNoteCommand createNoteCommand)
    {
        var domainEvent = new NoteCreatedEvent(createNoteCommand.NoteId, createNoteCommand.Text,
            createNoteCommand.NoteTypeId,
            createNoteCommand.TimeSlotId, createNoteCommand.TimeStamp);

        return CreateDatabaseEvent(nameof(NoteCreatedEvent), createNoteCommand.NoteId,
            JsonSerializer.Serialize(domainEvent));
    }

    public NoteEvent ToEvent(UpdateNoteCommand updateNoteCommand)
    {
        var domainEvent =
            new NoteUpdatedEvent(updateNoteCommand.NoteId, updateNoteCommand.Text, updateNoteCommand.NoteTypeId);

        return CreateDatabaseEvent(nameof(NoteUpdatedEvent), updateNoteCommand.NoteId,
            JsonSerializer.Serialize(domainEvent));
    }

    private static NoteEvent CreateDatabaseEvent(string eventName, Guid entityId, string json)
    {
        return new NoteEvent(Guid.NewGuid(), DateTime.UtcNow, TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow),
            eventName,
            entityId, json);
    }
}