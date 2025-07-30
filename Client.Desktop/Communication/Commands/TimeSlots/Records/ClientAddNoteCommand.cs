using System;

namespace Client.Desktop.Communication.Commands.TimeSlots.Records;

public record ClientAddNoteCommand(Guid TimeSlotId, Guid NoteId,
    Guid TraceId);