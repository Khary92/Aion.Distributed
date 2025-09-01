using Proto.Command.TimerSettings;
using Proto.DTO.TraceData;
using Proto.Notifications.TimerSettings;
using Service.Admin.Web.Communication.TimerSettings.Notifications;
using Service.Admin.Web.Communication.TimerSettings.Records;
using Service.Admin.Web.Communication.Wrappers;
using Service.Admin.Web.Models;

namespace Service.Admin.Web.Communication.TimerSettings;

public static class TimerSettingsExtensions
{
    public static CreateTimerSettingsCommandProto ToProto(this WebCreateTimerSettingsCommand command) =>
        new CreateTimerSettingsCommandProto
        {
            TimerSettingsId = command.TimerSettingsId.ToString(),
            DocumentationSaveInterval = command.DocumentationSaveInterval,
            SnapshotSaveInterval = command.SnapshotSaveInterval,
            TraceData = new TraceDataProto
            {
                TraceId = command.TraceId.ToString()
            }
        };

    public static ChangeDocuTimerSaveIntervalCommandProto ToProto(this WebChangeDocuTimerSaveIntervalCommand command) =>
        new()
        {
            TimerSettingsId = command.TimerSettingsId.ToString(),
            DocuTimerSaveInterval = command.DocuTimerSaveInterval,
            TraceData = new TraceDataProto
            {
                TraceId = command.TraceId.ToString()
            }
        };

    public static ChangeSnapshotSaveIntervalCommandProto ToProto(this WebChangeSnapshotSaveIntervalCommand command) =>
        new()
        {
            TimerSettingsId = command.TimerSettingsId.ToString(),
            SnapshotSaveInterval = command.SnapshotSaveInterval,
            TraceData = new TraceDataProto
            {
                TraceId = command.TraceId.ToString()
            }
        };

    public static NewTimerSettingsMessage ToNewEntityMessage(this TimerSettingsCreatedNotification notification) => new(
        new TimerSettingsWebModel(Guid.Parse(notification.TimerSettingsId),
            notification.DocumentationSaveInterval, notification.SnapshotSaveInterval),
        Guid.Parse(notification.TraceData.TraceId));

    public static WebDocuIntervalChangedNotification ToNotification(
        this DocuTimerSaveIntervalChangedNotification notification) => new(Guid.Parse(notification.TimerSettingsId),
        notification.DocuTimerSaveInterval, Guid.Parse(notification.TraceData.TraceId));

    public static WebSnapshotIntervalChangedNotification ToNotification(
        this SnapshotSaveIntervalChangedNotification notification) => new(notification.SnapshotSaveInterval,
        Guid.Parse(notification.TraceData.TraceId));
}