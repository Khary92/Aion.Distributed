using System.Threading.Tasks;
using Grpc.Net.Client;
using Proto.Command.StatisticsData;
using Proto.Command.Tags;

namespace Client.Avalonia.Communication.Sender;

public class TagCommandSender : ITagCommandSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempStatic.Address);
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