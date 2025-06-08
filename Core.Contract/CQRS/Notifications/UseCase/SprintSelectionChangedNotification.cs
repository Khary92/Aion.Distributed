using MediatR;

namespace Contract.CQRS.Notifications.UseCase;

public record SprintSelectionChangedNotification : INotification, IRequest, IRequest<Unit>;