namespace Service.Admin.Web.Communication.NoteType.Records.Notifications;

public record WebNoteTypeNameChangedNotification(Guid NoteTypeId, string Name, Guid TraceId);