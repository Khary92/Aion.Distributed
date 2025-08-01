using Core.Server.Communication.Records.Commands.Entities.TimerSettings;
using Proto.Command.TimerSettings;
using Proto.DTO.TimerSettings;
using Proto.Notifications.TimerSettings;

namespace Core.Server.Communication.Endpoints.TimerSettings;

public static class TimerSettingsProtoExtensions
{
    public static ChangeDocuTimerSaveIntervalCommand ToCommand(
        this ChangeDocuTimerSaveIntervalCommandProto proto) => new(Guid.Parse(proto.TimerSettingsId),
        proto.DocuTimerSaveInterval, Guid.Parse(proto.TraceData.TraceId));

    public static TimerSettingsNotification ToNotification(this ChangeDocuTimerSaveIntervalCommand proto) => new()
    {
        DocuTimerSaveIntervalChanged = new DocuTimerSaveIntervalChangedNotification
        {
            TimerSettingsId = proto.TimerSettingsId.ToString(),
            DocuTimerSaveInterval = proto.DocuTimerSaveInterval,
            TraceData = new()
            {
                TraceId = proto.TraceId.ToString()
            }
        }
    };

    public static ChangeSnapshotSaveIntervalCommand ToCommand(this ChangeSnapshotSaveIntervalCommandProto proto) => new(
        Guid.Parse(proto.TimerSettingsId), proto.SnapshotSaveInterval,
        Guid.Parse(proto.TraceData.TraceId));


    public static TimerSettingsNotification ToNotification(this ChangeSnapshotSaveIntervalCommand proto) => new()
    {
        SnapshotSaveIntervalChanged = new SnapshotSaveIntervalChangedNotification
        {
            TimerSettingsId = proto.TimerSettingsId.ToString(),
            SnapshotSaveInterval = proto.SnapshotSaveInterval,
            TraceData = new()
            {
                TraceId = proto.TraceId.ToString()
            }
        }
    };

    public static CreateTimerSettingsCommand ToCommand(this CreateTimerSettingsCommandProto proto) => new(
        Guid.Parse(proto.TimerSettingsId), proto.DocumentationSaveInterval,
        proto.SnapshotSaveInterval, Guid.Parse(proto.TraceData.TraceId));


    public static TimerSettingsNotification ToNotification(this CreateTimerSettingsCommand proto) => new()
    {
        TimerSettingsCreated = new TimerSettingsCreatedNotification
        {
            TimerSettingsId = proto.TimerSettingsId.ToString(),
            DocumentationSaveInterval = proto.DocumentationSaveInterval,
            SnapshotSaveInterval = proto.SnapshotSaveInterval,
            TraceData = new()
            {
                TraceId = proto.TraceId.ToString()
            }
        }
    };

    public static TimerSettingsProto ToProto(this Domain.Entities.TimerSettings timerSettings) => new()
    {
        TimerSettingsId = timerSettings.TimerSettingsId.ToString(),
        DocumentationSaveInterval = timerSettings.DocumentationSaveInterval,
        SnapshotSaveInterval = timerSettings.SnapshotSaveInterval
    };
}