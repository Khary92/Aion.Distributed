using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contract.DTO;
using Grpc.Net.Client;
using Proto.Requests.Notes;
using Proto.Shared;

namespace Client.Avalonia.Communication.Requests.Notes;

public class NotesRequestSender : INotesRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.Address);
    private readonly NotesRequestService.NotesRequestServiceClient _client = new(Channel);

    public async Task<List<NoteDto>> GetNotesByTicketId(string ticketId)
    {
        var request = new GetNotesByTicketIdRequestProto { TicketId = ticketId };
        var response = await _client.GetNotesByTicketIdAsync(request);

        return response.Notes.Select(note => new NoteDto(Guid.Parse(note.NoteId), note.Text,
            Guid.Parse(note.NoteTypeId), Guid.Parse(note.TimeSlotId), note.TimeStamp.ToDateTimeOffset())).ToList();
    }

    public async Task<List<NoteDto>> GetNotesByTimeSlotId(string timeSlotId)
    {
        var request = new GetNotesByTimeSlotIdRequestProto { TimeSlotId = timeSlotId };
        var response = await _client.GetNotesByTimeSlotIdAsync(request);
        
        return response.Notes.Select(note => new NoteDto(Guid.Parse(note.NoteId), note.Text,
            Guid.Parse(note.NoteTypeId), Guid.Parse(note.TimeSlotId), note.TimeStamp.ToDateTimeOffset())).ToList();
    }
}