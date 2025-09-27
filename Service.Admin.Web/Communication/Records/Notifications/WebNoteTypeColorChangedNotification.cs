namespace Service.Admin.Web.Communication.Records.Notifications;

public record WebNoteTypeColorChangedNotification(Guid NoteTypeId, string Color, Guid TraceId);