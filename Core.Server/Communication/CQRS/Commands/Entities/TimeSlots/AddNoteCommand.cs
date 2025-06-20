namespace Core.Server.Communication.CQRS.Commands.Entities.TimeSlots;

public record AddNoteCommand(Guid TimeSlotId, Guid NoteId);