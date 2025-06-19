using System.Threading.Tasks;
using Grpc.Net.Client;
using Proto.Command.Notes;
using Proto.Shared;

namespace Client.Desktop.Communication.Commands.Notes;

public class NoteCommandSender : INoteCommandSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly NoteCommandProtoService.NoteCommandProtoServiceClient _client = new(Channel);

    public async Task<bool> Send(CreateNoteCommandProto command)
    {
        var response = await _client.CreateNoteAsync(command);
        return response.Success;
    }

    public async Task<bool> Send(UpdateNoteCommandProto updateTicketDataCommand)
    {
        var response = await _client.UpdateNoteAsync(updateTicketDataCommand);
        return response.Success;
    }
    
}