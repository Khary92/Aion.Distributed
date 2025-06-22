using System.Text.Json;
using Core.Server.Communication.Records.Commands.Entities.Note;
using Domain.Events.Note;

namespace Core.Server.Translators.Commands.Notes;

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