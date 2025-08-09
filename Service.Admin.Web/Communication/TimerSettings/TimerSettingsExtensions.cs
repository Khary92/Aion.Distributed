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
    public static CreateTimerSettingsCommandProto ToProto(this WebCreateTimerSettingsCommand command)
    {
        return new CreateTimerSettingsCommandProto
        {
            TimerSettingsId = command.TimerSettingsId.ToString(),
            DocumentationSaveInterval = command.DocumentationSaveInterval,
            SnapshotSaveInterval = command.SnapshotSaveInterval,
            TraceData = new TraceDataProto
            {
                TraceId = command.TraceId.ToString()
            }
        };
    }

    public static ChangeDocuTimerSaveIntervalCommandProto ToProto(this WebChangeDocuTimerSaveIntervalCommand command)
    {
        return new ChangeDocuTimerSaveIntervalCommandProto
        {
            TimerSettingsId = command.TimerSettingsId.ToString(),
            DocuTimerSaveInterval = command.DocuTimerSaveInterval,
            TraceData = new TraceDataProto
            {
                TraceId = command.TraceId.ToString()
            }
        };
    }

    public static ChangeSnapshotSaveIntervalCommandProto ToProto(this WebChangeSnapshotSaveIntervalCommand command)
    {
        return new ChangeSnapshotSaveIntervalCommandProto
        {
            TimerSettingsId = command.TimerSettingsId.ToString(),
            SnapshotSaveInterval = command.SnapshotSaveInterval,
            TraceData = new TraceDataProto
            {
                TraceId = command.TraceId.ToString()
            }
        };
    }

    public static NewTimerSettingsMessage ToNewEntityMessage(this TimerSettingsCreatedNotification notification)
    {
        return new NewTimerSettingsMessage(new TimerSettingsWebModel(Guid.Parse(notification.TimerSettingsId),
            notification.DocumentationSaveInterval,
            notification.SnapshotSaveInterval), Guid.Parse(notification.TraceData.TraceId));
    }

    public static WebDocuIntervalChangedNotification ToNotification(
        this DocuTimerSaveIntervalChangedNotification notification)
    {
        return new WebDocuIntervalChangedNotification(
            Guid.Parse(notification.TimerSettingsId), notification.DocuTimerSaveInterval,
            Guid.Parse(notification.TraceData.TraceId));
    }

    public static WebSnapshotIntervalChangedNotification ToNotification(
        this SnapshotSaveIntervalChangedNotification notification)
    {
        return new WebSnapshotIntervalChangedNotification(Guid.Parse(notification.TimerSettingsId),
            notification.SnapshotSaveInterval, Guid.Parse(notification.TraceData.TraceId));
    }
}