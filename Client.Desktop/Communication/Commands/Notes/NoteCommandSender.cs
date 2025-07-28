using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.Notes.Records;
using Client.Proto;
using Grpc.Net.Client;
using Proto.Command.Notes;

namespace Client.Desktop.Communication.Commands.Notes;

public class NoteCommandSender : INoteCommandSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly NoteCommandProtoService.NoteCommandProtoServiceClient _client = new(Channel);

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