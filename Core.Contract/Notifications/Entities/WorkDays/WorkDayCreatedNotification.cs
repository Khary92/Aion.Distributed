using MediatR;

namespace Contract.Notifications.Entities.WorkDays;

public record WorkDayCreatedNotification(
    Guid WorkDayId,
    DateTimeOffset Date)
    : INotification, IRequest<Unit>;