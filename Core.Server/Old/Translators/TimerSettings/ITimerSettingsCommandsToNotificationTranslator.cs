using Service.Server.CQRS.Commands.Entities.TimerSettings;

namespace Service.Server.Old.Translators.TimerSettings;

public interface ITimerSettingsCommandsToNotificationTranslator
{
    DocuTimerSaveIntervalChangedNotification ToNotification(ChangeDocuTimerSaveIntervalCommand command);
    SnapshotSaveIntervalChangedNotification ToNotification(ChangeSnapshotSaveIntervalCommand command);
    TimerSettingsCreatedNotification ToNotification(CreateTimerSettingsCommand command);
}