using System;

namespace Client.Desktop.Communication.Notifications.Note.Records;

public record ClientNoteUpdatedNotification(Guid NoteId, string Text, Guid NoteTypeId, Guid TimeSlotId);