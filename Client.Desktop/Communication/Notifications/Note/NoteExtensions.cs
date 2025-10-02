using System;
using Client.Desktop.Communication.Notifications.Note.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.DataModels;
using Proto.Notifications.Note;

namespace Client.Desktop.Communication.Notifications.Note;

public static class NoteExtensions
{
    public static NewNoteMessage ToNewEntityMessage(this NoteCreatedNotification notification)
    {
        return new NewNoteMessage(
            new NoteClientModel(Guid.Parse(notification.NoteId), notification.Text, Guid.Parse(notification.NoteTypeId),
                Guid.Parse(notification.TimeSlotId), notification.TimeStamp.ToDateTimeOffset()),
            Guid.Parse(notification.TraceData.TraceId));
    }

    public static ClientNoteUpdatedNotification ToClientNotification(this NoteUpdatedNotification notification)
    {
        return new ClientNoteUpdatedNotification(
            Guid.Parse(notification.NoteId), notification.Text, Guid.Parse(notification.NoteTypeId),
            Guid.Parse(notification.TimeSlotId), Guid.Parse(notification.TraceData.TraceId));
    }
}