using MediatR;

namespace Contract.CQRS.Commands.Entities.TimerSettings;

public record CreateTimerSettingsCommand(
    Guid TimerSettingsId,
    int DocumentationSaveInterval,
    int SnapshotSaveInterval) : INotification, IRequest<Unit>;