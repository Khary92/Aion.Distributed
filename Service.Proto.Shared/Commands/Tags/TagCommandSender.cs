using Grpc.Net.Client;
using Proto.Command.Tags;

namespace Service.Proto.Shared.Commands.Tags;

public class TagCommandSender : ITagCommandSender
{
    private readonly TagCommandProtoService.TagCommandProtoServiceClient _client;

    public TagCommandSender(string address)
    {
        var channel = GrpcChannel.ForAddress(address);
        _client = new TagCommandProtoService.TagCommandProtoServiceClient(channel);
    }

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