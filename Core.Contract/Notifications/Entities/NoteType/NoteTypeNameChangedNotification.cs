using MediatR;

namespace Contract.Notifications.Entities.NoteType;

public record NoteTypeNameChangedNotification(Guid NoteTypeId, string Name) : INotification, IRequest<Unit>;