using Contract.CQRS.Notifications.Entities.NoteType;
using Proto.Notification.NoteType;

namespace Contract.Proto.Converter.Notifications;

public static class NoteTypeNotificationConverter
{
    public static NoteTypeColorChangedNotificationProto ToProto(this NoteTypeColorChangedNotification notification)
    {
        return new NoteTypeColorChangedNotificationProto
        {
            NoteTypeId = notification.NoteTypeId.ToString(),
            Color = notification.Color
        };
    }

    public static NoteTypeCreatedNotificationProto ToProto(this NoteTypeCreatedNotification notification)
    {
        return new NoteTypeCreatedNotificationProto
        {
            NoteTypeId = notification.NoteTypeId.ToString(),
            Name = notification.Name,
            Color = notification.Color
        };
    }

    public static NoteTypeNameChangedNotificationProto ToProto(this NoteTypeNameChangedNotification notification)
    {
        return new NoteTypeNameChangedNotificationProto
        {
            NoteTypeId = notification.NoteTypeId.ToString(),
            Name = notification.Name
        };
    }
}