using Contract.CQRS.Notifications.Entities.TimerSettings;
using Proto.Notification.TimerSettings;

namespace Contract.Proto.Converter.Notifications;

public static class TimerSettingsNotificationConverter
{
    public static DocuTimerSaveIntervalChangedNotificationProto ToProto(DocuTimerSaveIntervalChangedNotification notification)
        => new()
        {
            TimerSettingsId = notification.TimerSettingsId.ToString(),
            DocuTimerSaveInterval = notification.DocuTimerSaveInterval
        };

    public static DocuTimerSaveIntervalChangedNotification FromProto(DocuTimerSaveIntervalChangedNotificationProto proto)
        => new(
            Guid.Parse(proto.TimerSettingsId),
            proto.DocuTimerSaveInterval
        );

    public static SnapshotSaveIntervalChangedNotificationProto ToProto(SnapshotSaveIntervalChangedNotification notification)
        => new()
        {
            TimerSettingsId = notification.TimerSettingsId.ToString(),
            SnapshotSaveInterval = notification.SnapshotSaveInterval
        };

    public static SnapshotSaveIntervalChangedNotification FromProto(SnapshotSaveIntervalChangedNotificationProto proto)
        => new(
            Guid.Parse(proto.TimerSettingsId),
            proto.SnapshotSaveInterval
        );

    public static TimerSettingsCreatedNotificationProto ToProto(TimerSettingsCreatedNotification notification)
        => new()
        {
            TimerSettingsId = notification.TimerSettingsId.ToString(),
            DocumentationSaveInterval = notification.DocumentationSaveInterval,
            SnapshotSaveInterval = notification.SnapshotSaveInterval
        };

    public static TimerSettingsCreatedNotification FromProto(TimerSettingsCreatedNotificationProto proto)
        => new(
            Guid.Parse(proto.TimerSettingsId),
            proto.DocumentationSaveInterval,
            proto.SnapshotSaveInterval
        );
}