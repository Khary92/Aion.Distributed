using MediatR;

namespace Contract.Notifications.Entities.Tags;

public record TagUpdatedNotification(Guid TagId, string Name)
    : INotification, IRequest<Unit>;