using MediatR;

namespace Contract.CQRS.Notifications.Entities.NoteType;

public record NoteTypeNameChangedNotification(Guid NoteTypeId, string Name) : INotification, IRequest<Unit>;