using MediatR;

namespace Contract.CQRS.Notifications.Entities.Note;

public record NoteCreatedNotification(
    Guid NoteId,
    string Text,
    Guid NoteTypeId,
    Guid TimeSlotId,
    DateTimeOffset TimeStamp)
    : INotification, IRequest<Unit>;