using System.Threading.Tasks;
using Client.Avalonia.Communication.Commands;
using Grpc.Net.Client;
using Proto.Requests.Notes;

namespace Client.Avalonia.Communication.Requests;

public class NotesRequestSender : INotesRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempStatic.Address);
    private readonly NotesRequestService.NotesRequestServiceClient _client = new(Channel);

    public async Task<GetNotesResponseProto> GetNotesByTicketId(string ticketId)
    {
        var request = new GetNotesByTicketIdRequestProto { TicketId = ticketId };
        var response = await _client.GetNotesByTicketIdAsync(request);
        return response;
    }

    public async Task<GetNotesResponseProto> GetNotesByTimeSlotId(string timeSlotId)
    {
        var request = new GetNotesByTimeSlotIdRequestProto { TimeSlotId = timeSlotId };
        var response = await _client.GetNotesByTimeSlotIdAsync(request);
        return response;
    }
}