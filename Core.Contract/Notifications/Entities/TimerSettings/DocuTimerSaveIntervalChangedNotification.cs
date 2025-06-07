using MediatR;

namespace Contract.Notifications.Entities.TimerSettings;

public record DocuTimerSaveIntervalChangedNotification(Guid TimerSettingsId, int DocuTimerSaveInterval) : INotification, IRequest<Unit>;