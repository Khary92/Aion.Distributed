using MediatR;

namespace Contract.CQRS.Notifications.Entities.NoteType;

public record NoteTypeColorChangedNotification(Guid NoteTypeId, string Color) : INotification, IRequest<Unit>;