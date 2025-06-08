using Contract.DTO;

namespace Client.Avalonia.Communication.NotificationProcessors.Messages;

public record NewNoteTypeMessage(NoteTypeDto NoteType);