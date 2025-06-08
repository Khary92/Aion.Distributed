using MediatR;

namespace Contract.CQRS.Notifications.Entities.NoteType;

public record NoteTypeCreatedNotification(Guid NoteTypeId, string Name, string Color) : INotification, IRequest<Unit>;