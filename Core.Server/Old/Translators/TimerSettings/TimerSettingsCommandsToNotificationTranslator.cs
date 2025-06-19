using Service.Server.CQRS.Commands.Entities.TimerSettings;

namespace Service.Server.Old.Translators.TimerSettings;

public class TimerSettingsCommandsToNotificationTranslator : ITimerSettingsCommandsToNotificationTranslator
{
    public DocuTimerSaveIntervalChangedNotification ToNotification(ChangeDocuTimerSaveIntervalCommand command)
    {
        return new DocuTimerSaveIntervalChangedNotification(command.TimerSettingsId, command.DocuTimerSaveInterval);
    }

    public SnapshotSaveIntervalChangedNotification ToNotification(ChangeSnapshotSaveIntervalCommand command)
    {
        return new SnapshotSaveIntervalChangedNotification(command.TimerSettingsId, command.SnapshotSaveInterval);
    }

    public TimerSettingsCreatedNotification ToNotification(CreateTimerSettingsCommand command)
    {
        return new TimerSettingsCreatedNotification(command.TimerSettingsId, command.DocumentationSaveInterval,
            command.SnapshotSaveInterval);
    }
}