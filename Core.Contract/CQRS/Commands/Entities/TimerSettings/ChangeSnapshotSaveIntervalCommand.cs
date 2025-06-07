using MediatR;

namespace Contract.CQRS.Commands.Entities.TimerSettings;

public record ChangeSnapshotSaveIntervalCommand(
    Guid TimerSettingsId,
    int SnapshotSaveInterval) : INotification, IRequest<Unit>;