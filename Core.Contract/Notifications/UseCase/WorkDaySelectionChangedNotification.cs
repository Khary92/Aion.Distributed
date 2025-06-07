using MediatR;

namespace Contract.Notifications.UseCase;

public record WorkDaySelectionChangedNotification : INotification, IRequest;