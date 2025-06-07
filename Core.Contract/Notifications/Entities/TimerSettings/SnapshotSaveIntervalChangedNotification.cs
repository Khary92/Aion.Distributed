using MediatR;

namespace Contract.Notifications.Entities.TimerSettings;

public record SnapshotSaveIntervalChangedNotification(Guid TimerSettingsId, int SnapshotSaveInterval) : INotification, IRequest<Unit>;