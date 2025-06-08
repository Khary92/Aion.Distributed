using MediatR;

namespace Contract.CQRS.Notifications.Entities.TimerSettings;

public record DocuTimerSaveIntervalChangedNotification(Guid TimerSettingsId, int DocuTimerSaveInterval)
    : INotification, IRequest<Unit>;