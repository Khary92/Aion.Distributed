using System;

namespace Client.Desktop.Communication.Commands.Notes.Records;

public record ClientCreateNoteCommand(
    Guid NoteId,
    Guid NoteTypeId,
    string Text,
    Guid TimeSlotId,
    DateTimeOffset TimeStamp,
    Guid TraceId);