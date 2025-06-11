using System.Threading.Tasks;
using Proto.Requests.Notes;

namespace Client.Avalonia.Communication.Requests;

public interface INotesRequestSender
{
    Task<GetNotesResponseProto> GetNotesByTicketId(string ticketId);
    Task<GetNotesResponseProto> GetNotesByTimeSlotId(string timeSlotId);
}