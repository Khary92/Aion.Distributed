using MediatR;

namespace Contract.CQRS.Commands.Entities.TimerSettings;

public record ChangeDocuTimerSaveIntervalCommand(
    Guid TimerSettingsId,
    int DocuTimerSaveInterval) : INotification, IRequest<Unit>;