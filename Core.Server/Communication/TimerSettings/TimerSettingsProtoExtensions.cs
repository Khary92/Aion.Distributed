using Proto.Command.TimerSettings;
using Proto.DTO.TimerSettings;
using Proto.Notifications.TimerSettings;
using Service.Server.CQRS.Commands.Entities.TimerSettings;

namespace Service.Server.Communication.TimerSettings;

public static class TimerSettingsProtoExtensions
{
    public static ChangeDocuTimerSaveIntervalCommand ToCommand(
        this ChangeDocuTimerSaveIntervalCommandProto proto) =>
        new(Guid.Parse(proto.TimerSettingsId), proto.DocuTimerSaveInterval);

    public static TimerSettingsNotification ToNotification(this ChangeDocuTimerSaveIntervalCommand proto) =>
        new()
        {
            DocuTimerSaveIntervalChanged = new DocuTimerSaveIntervalChangedNotification()
            {
                TimerSettingsId = proto.TimerSettingsId.ToString(),
                DocuTimerSaveInterval = proto.DocuTimerSaveInterval
            }
        };
    
    public static ChangeSnapshotSaveIntervalCommand ToCommand(
        this ChangeSnapshotSaveIntervalCommandProto proto) =>
        new(Guid.Parse(proto.TimerSettingsId), proto.SnapshotSaveInterval);

    public static TimerSettingsNotification ToNotification(this ChangeSnapshotSaveIntervalCommand proto) =>
        new()
        {
            SnapshotSaveIntervalChanged = new SnapshotSaveIntervalChangedNotification
            {
                TimerSettingsId = proto.TimerSettingsId.ToString(),
                SnapshotSaveInterval = proto.SnapshotSaveInterval
            }
        };
    
    public static CreateTimerSettingsCommand ToCommand(
        this CreateTimerSettingsCommandProto proto) =>
        new(Guid.Parse(proto.TimerSettingsId),proto.DocumentationSaveInterval, proto.SnapshotSaveInterval);

    public static TimerSettingsNotification ToNotification(this CreateTimerSettingsCommand proto) =>
        new()
        {
            TimerSettingsCreated = new TimerSettingsCreatedNotification
            {
                TimerSettingsId = proto.TimerSettingsId.ToString(),
                DocumentationSaveInterval = proto.DocumentationSaveInterval,
                SnapshotSaveInterval = proto.SnapshotSaveInterval
            }
        };

    public static TimerSettingsProto ToProto(this Domain.Entities.TimerSettings timerSettings) =>
        new()
        {
            TimerSettingsId = timerSettings.TimerSettingsId.ToString(),
            DocumentationSaveInterval = timerSettings.DocumentationSaveInterval,
            SnapshotSaveInterval = timerSettings.SnapshotSaveInterval,
        };
}