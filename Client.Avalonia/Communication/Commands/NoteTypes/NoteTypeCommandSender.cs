using System.Threading.Tasks;
using Grpc.Net.Client;
using Proto.Command.NoteTypes;

namespace Client.Avalonia.Communication.Commands.NoteTypes;

public class NoteTypeCommandSender : INoteTypeCommandSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.Address);
    private readonly NoteTypeCommandService.NoteTypeCommandServiceClient _client = new(Channel);

    public async Task<bool> Send(CreateNoteTypeCommand command)
    {
        var response = await _client.CreateNoteTypeAsync(command);
        return response.Success;
    }

    public async Task<bool> Send(ChangeNoteTypeNameCommand command)
    {
        var response = await _client.ChangeNoteTypeNameAsync(command);
        return response.Success;
    }
    
    public async Task<bool> Send(ChangeNoteTypeColorCommand command)
    {
        var response = await _client.ChangeNoteTypeColorAsync(command);
        return response.Success;
    }
}