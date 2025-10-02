using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.Notes.Records;
using Grpc.Net.Client;
using Proto.Command.Notes;

namespace Client.Desktop.Communication.Commands.Notes;

public class NoteCommandSender : INoteCommandSender
{
    private readonly NoteCommandProtoService.NoteCommandProtoServiceClient _client;

    public NoteCommandSender(string address)
    {
        var channel = GrpcChannel.ForAddress(address);
        _client = new NoteCommandProtoService.NoteCommandProtoServiceClient(channel);
    }

    public async Task<bool> Send(ClientCreateNoteCommand command)
    {
        var response = await _client.CreateNoteAsync(command.ToProto());
        return response.Success;
    }

    public async Task<bool> Send(ClientUpdateNoteCommand command)
    {
        var response = await _client.UpdateNoteAsync(command.ToProto());
        return response.Success;
    }
}