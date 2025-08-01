using Proto.Notifications.NoteType;
using Service.Admin.Web.Communication.NoteType.Notifications;
using Service.Admin.Web.Communication.Wrappers;
using Service.Admin.Web.Models;

namespace Service.Admin.Web.Communication.NoteType;

public static class NoteTypeExtensions
{
    public static NewNoteTypeMessage ToWebModel(this NoteTypeCreatedNotification notification) => new(
        new NoteTypeWebModel(
            Guid.Parse(notification.NoteTypeId), notification.Name, notification.Color),
        Guid.Parse(notification.TraceData.TraceId));

    public static WebNoteTypeColorChangedNotification
        ToNotification(this NoteTypeColorChangedNotification notification) => new(
        Guid.Parse(notification.NoteTypeId), notification.Color, Guid.Parse(notification.TraceData.TraceId));

    public static WebNoteTypeNameChangedNotification
        ToNotification(this NoteTypeNameChangedNotification notification) => new(Guid.Parse(notification.NoteTypeId),
        notification.Name, Guid.Parse(notification.TraceData.TraceId));
}