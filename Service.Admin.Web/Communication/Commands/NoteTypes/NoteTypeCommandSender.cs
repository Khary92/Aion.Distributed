using Grpc.Net.Client;
using Proto.Command.NoteTypes;

namespace Service.Admin.Web.Communication.Commands.NoteTypes;

public class NoteTypeCommandSender : INoteTypeCommandSender
{
    private readonly NoteTypeProtoCommandService.NoteTypeProtoCommandServiceClient _client;

    public NoteTypeCommandSender(string address)
    {
        var channel = GrpcChannel.ForAddress(address);
        _client = new NoteTypeProtoCommandService.NoteTypeProtoCommandServiceClient(channel);
    }

    public async Task<bool> Send(CreateNoteTypeCommandProto command)
    {
        var response = await _client.CreateNoteTypeAsync(command);
        return response.Success;
    }

    public async Task<bool> Send(ChangeNoteTypeNameCommandProto command)
    {
        var response = await _client.ChangeNoteTypeNameAsync(command);
        return response.Success;
    }

    public async Task<bool> Send(ChangeNoteTypeColorCommandProto command)
    {
        var response = await _client.ChangeNoteTypeColorAsync(command);
        return response.Success;
    }
}