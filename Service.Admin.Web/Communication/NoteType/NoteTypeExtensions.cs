using Proto.Notifications.NoteType;
using Service.Admin.Web.Communication.NoteType.Notifications;
using Service.Admin.Web.DTO;

namespace Service.Admin.Web.Communication.NoteType;

public static class NoteTypeExtensions
{
    public static NoteTypeDto ToDto(this NoteTypeCreatedNotification notification)
    {
        return new NoteTypeDto(Guid.Parse(notification.NoteTypeId), notification.Name, notification.Color);
    }

    public static WebNoteTypeColorChangedNotification ToNotification(this NoteTypeColorChangedNotification notification)
    {
        return new WebNoteTypeColorChangedNotification(Guid.Parse(notification.NoteTypeId), notification.Color);
    }
    
    public static WebNoteTypeNameChangedNotification ToNotification(this NoteTypeNameChangedNotification notification)
    {
        return new WebNoteTypeNameChangedNotification(Guid.Parse(notification.NoteTypeId), notification.Name);
    }
}