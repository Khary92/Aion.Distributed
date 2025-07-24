namespace Service.Admin.Web.Communication.NoteType.Notifications;

public record WebNoteTypeNameChangedNotification(Guid NoteTypeId, string Name);