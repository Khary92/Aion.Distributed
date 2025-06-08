using MediatR;

namespace Contract.CQRS.Notifications.Entities.Tags;

public record TagUpdatedNotification(Guid TagId, string Name)
    : INotification, IRequest<Unit>;