using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.Notes;
using Client.Desktop.Communication.Requests.Notes.Records;
using Client.Desktop.DataModels;

namespace Client.Desktop.Communication.Mock.Requests;

public class MockNotesRequestSender(MockDataService mockDataService) : INotesRequestSender
{
    public Task<List<NoteClientModel>> Send(ClientGetNotesByTicketIdRequest request)
    {
        return Task.FromResult(mockDataService.Notes.Where(n => n.TicketId == request.TicketId).ToList());
    }

    public Task<List<NoteClientModel>> Send(ClientGetNotesByTimeSlotIdRequest request)
    {
        return Task.FromResult(mockDataService.Notes.Where(n => n.TimeSlotId == request.TimeSlotId).ToList());
    }
}