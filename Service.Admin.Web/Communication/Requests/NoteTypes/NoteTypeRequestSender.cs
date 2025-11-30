using Grpc.Core;
using Grpc.Net.Client;
using Proto.DTO.NoteType;
using Proto.Requests.NoteTypes;
using Service.Admin.Web.Communication.Authentication;

namespace Service.Admin.Web.Communication.Requests.NoteTypes;

public class NoteTypeRequestSender : INoteTypeRequestSender
{
    private readonly JwtService _jwtService;
    private readonly NoteTypeProtoRequestService.NoteTypeProtoRequestServiceClient _client;

    public NoteTypeRequestSender(string address, JwtService jwtService)
    {
        _jwtService = jwtService;
        var channel = GrpcChannel.ForAddress(address);
        _client = new NoteTypeProtoRequestService.NoteTypeProtoRequestServiceClient(channel);
    }

    private Metadata GetAuthHeader() => new()
    {
        { "Authorization", $"Bearer {_jwtService.Token}" }
    };
    
    public async Task<GetAllNoteTypesResponseProto> Send(GetAllNoteTypesRequestProto request)
    {
        return await _client.GetAllNoteTypesAsync(request, GetAuthHeader());
    }

    public async Task<NoteTypeProto> Send(GetNoteTypeByIdRequestProto request)
    {
        return await _client.GetNoteTypeByIdAsync(request, GetAuthHeader());
    }
}