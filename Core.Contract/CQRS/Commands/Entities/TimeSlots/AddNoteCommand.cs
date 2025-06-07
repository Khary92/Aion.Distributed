using MediatR;

namespace Contract.CQRS.Commands.Entities.TimeSlots;

public record AddNoteCommand(Guid TimeSlotId, Guid NoteId) : INotification, IRequest<Unit>;