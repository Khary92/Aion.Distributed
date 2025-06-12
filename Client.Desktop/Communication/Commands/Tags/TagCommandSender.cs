using System.Threading.Tasks;
using Grpc.Net.Client;
using Proto.Command.Tags;
using Proto.Shared;

namespace Client.Desktop.Communication.Commands.Tags;

public class TagCommandSender : ITagCommandSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly TagCommandService.TagCommandServiceClient _client = new(Channel);

    public async Task<bool> Send(CreateTagCommand command)
    {
        var response = await _client.CreateTagAsync(command);
        return response.Success;
    }

    public async Task<bool> Send(UpdateTagCommand command)
    {
        var response = await _client.UpdateTagAsync(command);
        return response.Success;
    }
}