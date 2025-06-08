using Contract.CQRS.Notifications.Entities.Tags;
using Proto.Notification.Tags;

namespace Contract.Proto.Converter.Notifications;

public static class TagNotificationConverter
{
    public static TagCreatedNotificationProto ToProto(TagCreatedNotification notification)
        => new()
        {
            TagId = notification.TagId.ToString(),
            Name = notification.Name
        };

    public static TagCreatedNotification FromProto(TagCreatedNotificationProto proto)
        => new(Guid.Parse(proto.TagId), proto.Name);

    public static TagUpdatedNotificationProto ToProto(TagUpdatedNotification notification)
        => new()
        {
            TagId = notification.TagId.ToString(),
            Name = notification.Name
        };

    public static TagUpdatedNotification FromProto(TagUpdatedNotificationProto proto)
        => new(Guid.Parse(proto.TagId), proto.Name);
}