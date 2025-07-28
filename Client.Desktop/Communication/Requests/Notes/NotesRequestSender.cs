using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.Notes.Records;
using Client.Desktop.DataModels;
using Client.Proto;
using Grpc.Net.Client;
using Proto.Requests.Notes;

namespace Client.Desktop.Communication.Requests.Notes;

public class NotesRequestSender : INotesRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly NotesRequestService.NotesRequestServiceClient _client = new(Channel);

    public async Task<List<NoteClientModel>> Send(ClientGetNotesByTicketIdRequest request)
    {
        var response = await _client.GetNotesByTicketIdAsync(request.ToProto());
        return response.ToClientModelList();
    }

    public async Task<List<NoteClientModel>> Send(ClientGetNotesByTimeSlotIdRequest request)
    {
        var response = await _client.GetNotesByTimeSlotIdAsync(request.ToProto());
        return response.ToClientModelList();
    }
}