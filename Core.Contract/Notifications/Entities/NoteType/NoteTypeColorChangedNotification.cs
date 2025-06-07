using MediatR;

namespace Contract.Notifications.Entities.NoteType;

public record NoteTypeColorChangedNotification(Guid NoteTypeId, string Color) : INotification, IRequest<Unit>;