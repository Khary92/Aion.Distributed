using System.Threading.Tasks;
using Client.Desktop.Proto;
using Grpc.Net.Client;
using Proto.Command.NoteTypes;

namespace Client.Desktop.Communication.Commands.NoteTypes;

public class NoteTypeCommandSender : INoteTypeCommandSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly NoteTypeCommandProtoService.NoteTypeCommandProtoServiceClient _client = new(Channel);

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