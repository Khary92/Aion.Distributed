using System;

namespace Client.Desktop.Communication.Requests.Notes.Records;

public record ClientGetNotesByTicketIdRequest(Guid TicketId);