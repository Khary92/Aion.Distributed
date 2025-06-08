using MediatR;

namespace Contract.CQRS.Notifications.Entities.WorkDays;

public record WorkDayCreatedNotification(
    Guid WorkDayId,
    DateTimeOffset Date)
    : INotification, IRequest<Unit>;