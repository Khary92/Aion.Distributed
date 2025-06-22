namespace Core.Server.Communication.Records.Commands.Entities.TimeSlots;

public record AddNoteCommand(Guid TimeSlotId, Guid NoteId);