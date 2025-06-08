using System;
using Contract.CQRS.Notifications.Entities.Note;
using Proto.Notification.Note;

public static class NoteNotificationConverter
{
    public static NoteCreatedNotificationProto ToProto(this NoteCreatedNotification notification)
    {
        return new NoteCreatedNotificationProto
        {
            NoteId = notification.NoteId.ToString(),
            Text = notification.Text,
            NoteTypeId = notification.NoteTypeId.ToString(),
            TimeSlotId = notification.TimeSlotId.ToString(),
            TimeStamp = notification.TimeStamp.ToString("o") // ISO 8601
        };
    }

    public static NoteUpdatedNotificationProto ToProto(this NoteUpdatedNotification notification)
    {
        return new NoteUpdatedNotificationProto
        {
            NoteId = notification.NoteId.ToString(),
            Text = notification.Text,
            NoteTypeId = notification.NoteTypeId.ToString(),
            TimeSlotId = notification.TimeSlotId.ToString()
        };
    }
}