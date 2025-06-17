using Application.Contract.CQRS.Commands.Entities.TimerSettings;
using Application.Contract.Notifications.Entities.TimerSettings;

namespace Application.Translators.TimerSettings;

public interface ITimerSettingsCommandsToNotificationTranslator
{
    DocuTimerSaveIntervalChangedNotification ToNotification(ChangeDocuTimerSaveIntervalCommand command);
    SnapshotSaveIntervalChangedNotification ToNotification(ChangeSnapshotSaveIntervalCommand command);
    TimerSettingsCreatedNotification ToNotification(CreateTimerSettingsCommand command);
}