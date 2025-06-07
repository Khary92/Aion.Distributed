using MediatR;

namespace Contract.Notifications.UseCase;

public record SprintSelectionChangedNotification : INotification, IRequest, IRequest<Unit>;