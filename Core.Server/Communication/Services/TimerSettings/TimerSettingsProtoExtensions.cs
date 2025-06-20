using Core.Server.Communication.CQRS.Commands.Entities.TimerSettings;
using Proto.Command.TimerSettings;
using Proto.DTO.TimerSettings;
using Proto.Notifications.TimerSettings;

namespace Core.Server.Communication.Services.TimerSettings;

public static class TimerSettingsProtoExtensions
{
    public static ChangeDocuTimerSaveIntervalCommand ToCommand(
        this ChangeDocuTimerSaveIntervalCommandProto proto)
    {
        return new ChangeDocuTimerSaveIntervalCommand(Guid.Parse(proto.TimerSettingsId), proto.DocuTimerSaveInterval);
    }

    public static TimerSettingsNotification ToNotification(this ChangeDocuTimerSaveIntervalCommand proto)
    {
        return new TimerSettingsNotification
        {
            DocuTimerSaveIntervalChanged = new DocuTimerSaveIntervalChangedNotification
            {
                TimerSettingsId = proto.TimerSettingsId.ToString(),
                DocuTimerSaveInterval = proto.DocuTimerSaveInterval
            }
        };
    }

    public static ChangeSnapshotSaveIntervalCommand ToCommand(
        this ChangeSnapshotSaveIntervalCommandProto proto)
    {
        return new ChangeSnapshotSaveIntervalCommand(Guid.Parse(proto.TimerSettingsId), proto.SnapshotSaveInterval);
    }

    public static TimerSettingsNotification ToNotification(this ChangeSnapshotSaveIntervalCommand proto)
    {
        return new TimerSettingsNotification
        {
            SnapshotSaveIntervalChanged = new SnapshotSaveIntervalChangedNotification
            {
                TimerSettingsId = proto.TimerSettingsId.ToString(),
                SnapshotSaveInterval = proto.SnapshotSaveInterval
            }
        };
    }

    public static CreateTimerSettingsCommand ToCommand(
        this CreateTimerSettingsCommandProto proto)
    {
        return new CreateTimerSettingsCommand(Guid.Parse(proto.TimerSettingsId), proto.DocumentationSaveInterval,
            proto.SnapshotSaveInterval);
    }

    public static TimerSettingsNotification ToNotification(this CreateTimerSettingsCommand proto)
    {
        return new TimerSettingsNotification
        {
            TimerSettingsCreated = new TimerSettingsCreatedNotification
            {
                TimerSettingsId = proto.TimerSettingsId.ToString(),
                DocumentationSaveInterval = proto.DocumentationSaveInterval,
                SnapshotSaveInterval = proto.SnapshotSaveInterval
            }
        };
    }

    public static TimerSettingsProto ToProto(this Domain.Entities.TimerSettings timerSettings)
    {
        return new TimerSettingsProto
        {
            TimerSettingsId = timerSettings.TimerSettingsId.ToString(),
            DocumentationSaveInterval = timerSettings.DocumentationSaveInterval,
            SnapshotSaveInterval = timerSettings.SnapshotSaveInterval
        };
    }
}