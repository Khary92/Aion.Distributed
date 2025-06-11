using System.Collections.Generic;
using System.Threading.Tasks;
using Contract.DTO;
using Proto.Requests.Notes;

namespace Client.Avalonia.Communication.Requests.Notes;

public interface INotesRequestSender
{
    Task<List<NoteDto>> GetNotesByTicketId(string ticketId);
    Task<List<NoteDto>> GetNotesByTimeSlotId(string timeSlotId);
}