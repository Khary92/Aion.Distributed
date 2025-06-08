using MediatR;

namespace Contract.CQRS.Notifications.Entities.Tags;

public record TagCreatedNotification(Guid TagId, string Name)
    : INotification, IRequest<Unit>;