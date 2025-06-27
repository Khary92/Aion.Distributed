using System.Threading.Tasks;
using Client.Desktop.Proto;
using Grpc.Net.Client;
using Proto.Command.Tags;

namespace Client.Desktop.Communication.Commands.Tags;

public class TagCommandSender : ITagCommandSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly TagCommandProtoService.TagCommandProtoServiceClient _client = new(Channel);

    public async Task<bool> Send(CreateTagCommandProto command)
    {
        var response = await _client.CreateTagAsync(command);
        return response.Success;
    }

    public async Task<bool> Send(UpdateTagCommandProto command)
    {
        var response = await _client.UpdateTagAsync(command);
        return response.Success;
    }
}