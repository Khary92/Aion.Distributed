using Contract.CQRS.Notifications.Entities.Sprints;
using Proto.Notification.Sprints;

namespace Contract.Proto.Converter.Notifications;

public static class SprintNotificationConverter
{
    public static SprintActiveStatusSetNotificationProto ToProto(SprintActiveStatusSetNotification notification)
        => new()
        {
            SprintId = notification.SprintId.ToString(),
            IsActive = notification.IsActive
        };

    public static SprintActiveStatusSetNotification FromProto(SprintActiveStatusSetNotificationProto proto)
        => new(Guid.Parse(proto.SprintId), proto.IsActive);

    public static SprintCreatedNotificationProto ToProto(SprintCreatedNotification notification)
        => new()
        {
            SprintId = notification.SprintId.ToString(),
            Name = notification.Name,
            StartTime = notification.StartTime.ToString("o"),
            EndTime = notification.EndTime.ToString("o"),
            IsActive = notification.IsActive,
            TicketIds = { notification.TicketIds.Select(g => g.ToString()) }
        };

    public static SprintCreatedNotification FromProto(SprintCreatedNotificationProto proto)
        => new(
            Guid.Parse(proto.SprintId),
            proto.Name,
            DateTimeOffset.Parse(proto.StartTime),
            DateTimeOffset.Parse(proto.EndTime),
            proto.IsActive,
            proto.TicketIds.Select(Guid.Parse).ToList()
        );

    public static SprintDataUpdatedNotificationProto ToProto(SprintDataUpdatedNotification notification)
        => new()
        {
            SprintId = notification.SprintId.ToString(),
            Name = notification.Name,
            StartTime = notification.StartTime.ToString("o"),
            EndTime = notification.EndTime.ToString("o")
        };

    public static SprintDataUpdatedNotification FromProto(SprintDataUpdatedNotificationProto proto)
        => new(
            Guid.Parse(proto.SprintId),
            proto.Name,
            DateTimeOffset.Parse(proto.StartTime),
            DateTimeOffset.Parse(proto.EndTime)
        );

    public static TicketAddedToActiveSprintNotificationProto ToProto(TicketAddedToActiveSprintNotification notification)
        => new();

    public static TicketAddedToActiveSprintNotification FromProto(TicketAddedToActiveSprintNotificationProto proto)
        => new();

    public static TicketAddedToSprintNotificationProto ToProto(TicketAddedToSprintNotification notification)
        => new()
        {
            SprintId = notification.SprintId.ToString(),
            TicketId = notification.TicketId.ToString()
        };

    public static TicketAddedToSprintNotification FromProto(TicketAddedToSprintNotificationProto proto)
        => new(Guid.Parse(proto.SprintId), Guid.Parse(proto.TicketId));
}