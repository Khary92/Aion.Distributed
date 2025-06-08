using MediatR;

namespace Contract.CQRS.Notifications.Entities.TimerSettings;

public record SnapshotSaveIntervalChangedNotification(Guid TimerSettingsId, int SnapshotSaveInterval)
    : INotification, IRequest<Unit>;