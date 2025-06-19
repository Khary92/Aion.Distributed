
namespace Service.Server.CQRS.Commands.Entities.TimeSlots;

public record AddNoteCommand(Guid TimeSlotId, Guid NoteId);