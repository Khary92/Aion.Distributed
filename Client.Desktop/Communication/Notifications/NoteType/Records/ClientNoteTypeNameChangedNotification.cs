using System;

namespace Client.Desktop.Communication.Notifications.NoteType.Records;

public record ClientNoteTypeNameChangedNotification(Guid NoteTypeId, string Name, Guid TraceId);