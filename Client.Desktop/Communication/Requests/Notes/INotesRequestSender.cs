using System.Collections.Generic;
using System.Threading.Tasks;
using Contract.DTO;
using Proto.Requests.Notes;

namespace Client.Desktop.Communication.Requests.Notes;

public interface INotesRequestSender
{
    Task<List<NoteDto>> Send(GetNotesByTicketIdRequestProto request);
    Task<List<NoteDto>> Send(GetNotesByTimeSlotIdRequestProto request);
}