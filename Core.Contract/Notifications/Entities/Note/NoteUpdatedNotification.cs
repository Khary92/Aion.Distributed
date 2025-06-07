using MediatR;

namespace Contract.Notifications.Entities.Note;

public record NoteUpdatedNotification(Guid NoteId, string Text, Guid NoteTypeId, Guid TimeSlotId)
    : INotification, IRequest<Unit>;