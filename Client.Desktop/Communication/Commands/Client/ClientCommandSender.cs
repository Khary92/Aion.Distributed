using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.Client.Records;
using Grpc.Net.Client;
using Proto.Command.Client;

namespace Client.Desktop.Communication.Commands.Client;

public class ClientCommandSender : IClientCommandSender
{
    private readonly ClientCommandProtoService.ClientCommandProtoServiceClient _client;

    public ClientCommandSender(string address)
    {
        var channel = GrpcChannel.ForAddress(address);
        _client = new ClientCommandProtoService.ClientCommandProtoServiceClient(channel);
    }

    public async Task<bool> Send(ClientCreateTrackingControlCommand command)
    {
        var response = await _client.CreateTimeSlotControlAsync(command.ToProto());
        return response.Success;
    }
}