using MediatR;

namespace Contract.CQRS.Notifications.UseCase;

public record CreateSnapshotNotification : INotification;