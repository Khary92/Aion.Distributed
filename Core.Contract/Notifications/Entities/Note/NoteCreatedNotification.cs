using MediatR;

namespace Contract.Notifications.Entities.Note;

public record NoteCreatedNotification(
    Guid NoteId,
    string Text,
    Guid NoteTypeId,
    Guid TimeSlotId,
    DateTimeOffset TimeStamp)
    : INotification, IRequest<Unit>;