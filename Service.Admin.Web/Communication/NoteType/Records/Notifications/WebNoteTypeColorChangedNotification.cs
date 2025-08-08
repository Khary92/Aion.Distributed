namespace Service.Admin.Web.Communication.NoteType.Notifications;

public record WebNoteTypeColorChangedNotification(Guid NoteTypeId, string Color, Guid TraceId);