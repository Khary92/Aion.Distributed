using System;
using Contract.CQRS.Notifications.Entities.WorkDays;
using Proto.Notification.WorkDays;

public static class WorkDayNotificationConverter
{
    public static WorkDayCreatedNotificationProto ToProto(WorkDayCreatedNotification notification)
        => new()
        {
            WorkDayId = notification.WorkDayId.ToString(),
            Date = notification.Date.ToString("o") // ISO 8601
        };

    public static WorkDayCreatedNotification FromProto(WorkDayCreatedNotificationProto proto)
        => new(
            Guid.Parse(proto.WorkDayId),
            DateTimeOffset.Parse(proto.Date)
        );
}