using System;

namespace Client.Desktop.Communication.Requests.NoteType;

public record ClientGetNoteTypeByIdRequest(Guid NoteTypeId);