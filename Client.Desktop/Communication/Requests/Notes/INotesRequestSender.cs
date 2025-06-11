using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contract.DTO;

namespace Client.Desktop.Communication.Requests.Notes;

public interface INotesRequestSender
{
    Task<List<NoteDto>> GetNotesByTicketId(Guid ticketId);
    Task<List<NoteDto>> GetNotesByTimeSlotId(string timeSlotId);
}