using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.Notes.Records;
using Client.Desktop.DataModels;

namespace Client.Desktop.Communication.Requests.Notes;

public interface INotesRequestSender
{
    Task<List<NoteClientModel>> Send(ClientGetNotesByTicketIdRequest request);
    Task<List<NoteClientModel>> Send(ClientGetNotesByTimeSlotIdRequest request);
}