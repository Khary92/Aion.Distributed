using Proto.Command.TimerSettings;
using Proto.DTO.TraceData;
using Proto.Notifications.TimerSettings;
using Service.Admin.Web.Communication.Records.Commands;
using Service.Admin.Web.Communication.Records.Notifications;
using Service.Admin.Web.Communication.Records.Wrappers;
using Service.Admin.Web.Models;

namespace Service.Admin.Web.Communication.Mapper;

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
        return new NewTimerSettingsMessage(
            new TimerSettingsWebModel(Guid.Parse(notification.TimerSettingsId),
                notification.DocumentationSaveInterval, notification.SnapshotSaveInterval),
            Guid.Parse(notification.TraceData.TraceId));
    }

    public static WebDocuIntervalChangedNotification ToNotification(
        this DocuTimerSaveIntervalChangedNotification notification)
    {
        return new WebDocuIntervalChangedNotification(notification.DocuTimerSaveInterval,
            Guid.Parse(notification.TraceData.TraceId));
    }

    public static WebSnapshotIntervalChangedNotification ToNotification(
        this SnapshotSaveIntervalChangedNotification notification)
    {
        return new WebSnapshotIntervalChangedNotification(notification.SnapshotSaveInterval,
            Guid.Parse(notification.TraceData.TraceId));
    }
}