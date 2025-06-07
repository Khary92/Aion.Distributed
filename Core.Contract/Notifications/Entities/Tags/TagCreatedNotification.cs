using MediatR;

namespace Contract.Notifications.Entities.Tags;

public record TagCreatedNotification(Guid TagId, string Name)
    : INotification, IRequest<Unit>;