using MediatR;

namespace Contract.Notifications.Entities.TimerSettings;

public record TimerSettingsCreatedNotification(
    Guid TimerSettingsId,
    int DocumentationSaveInterval,
    int SnapshotSaveInterval) : INotification, IRequest<Unit>;