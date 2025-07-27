using System;

namespace Client.Desktop.Communication.Commands.Notes.Records;

public record ClientUpdateNoteCommand(Guid NoteId, string Text, Guid NoteTypeId, Guid TimeSlotId);