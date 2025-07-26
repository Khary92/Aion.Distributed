using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.DataModels;
using Proto.Requests.Notes;

namespace Client.Desktop.Communication.Requests.Notes;

public interface INotesRequestSender
{
    Task<List<NoteClientModel>> Send(GetNotesByTicketIdRequestProto request);
    Task<List<NoteClientModel>> Send(GetNotesByTimeSlotIdRequestProto request);
}