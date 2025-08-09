namespace Service.Admin.Web.Communication.NoteType.Records.Notifications;

public record WebNoteTypeColorChangedNotification(Guid NoteTypeId, string Color, Guid TraceId);