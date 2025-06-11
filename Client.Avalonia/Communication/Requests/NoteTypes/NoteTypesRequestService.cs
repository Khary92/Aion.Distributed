using System.Threading.Tasks;
using Client.Avalonia.Communication.Commands;
using Grpc.Net.Client;
using Proto.Requests.NoteTypes;

namespace Client.Avalonia.Communication.Requests.NoteTypes;

public class NoteTypesRequestSender : INoteTypesRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.Address);
    private readonly NoteTypesRequestService.NoteTypesRequestServiceClient _client = new(Channel);

    public async Task<GetAllNoteTypesResponseProto> GetAllNoteTypes()
    {
        var request = new GetAllNoteTypesRequestProto();
        var response = await _client.GetAllNoteTypesAsync(request);
        return response;
    }

    public async Task<NoteTypeProto> GetNoteTypeById(string noteTypeId)
    {
        var request = new GetNoteTypeByIdRequestProto { NoteTypeId = noteTypeId };
        var response = await _client.GetNoteTypeByIdAsync(request);
        return response;
    }
}