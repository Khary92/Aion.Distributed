using System;
using Client.Desktop.Communication.Notifications.NoteType.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.DataModels;
using Proto.Notifications.NoteType;

namespace Client.Desktop.Communication.Notifications.NoteType;

public static class NoteTypeExtensions
{
    public static NewNoteTypeMessage ToNewEntityMessage(this NoteTypeCreatedNotification notification)
    {
        return new NewNoteTypeMessage(new NoteTypeClientModel(Guid.Parse(notification.NoteTypeId), notification.Name,
            notification.Color), Guid.Parse(notification.TraceData.TraceId));
    }

    public static ClientNoteTypeColorChangedNotification ToClientNotification(
        this NoteTypeColorChangedNotification notification)
    {
        return new ClientNoteTypeColorChangedNotification(Guid.Parse(notification.NoteTypeId), notification.Color,
            Guid.Parse(notification.TraceData.TraceId));
    }

    public static ClientNoteTypeNameChangedNotification ToClientNotification(
        this NoteTypeNameChangedNotification notification)
    {
        return new ClientNoteTypeNameChangedNotification(Guid.Parse(notification.NoteTypeId), notification.Name,
            Guid.Parse(notification.TraceData.TraceId));
    }
}