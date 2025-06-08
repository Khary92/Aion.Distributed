using MediatR;

namespace Contract.CQRS.Notifications.Entities.TimerSettings;

public record TimerSettingsCreatedNotification(
    Guid TimerSettingsId,
    int DocumentationSaveInterval,
    int SnapshotSaveInterval) : INotification, IRequest<Unit>;