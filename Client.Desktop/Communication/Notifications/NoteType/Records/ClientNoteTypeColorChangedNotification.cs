using System;

namespace Client.Desktop.Communication.Notifications.NoteType.Records;

public record ClientNoteTypeColorChangedNotification(Guid NoteTypeId, string Color);