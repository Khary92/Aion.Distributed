namespace Service.Admin.Web.Communication.Records.Notifications;

public record WebNoteTypeNameChangedNotification(Guid NoteTypeId, string Name, Guid TraceId);