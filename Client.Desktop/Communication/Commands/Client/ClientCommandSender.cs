using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.Client.Records;
using Client.Proto;
using Grpc.Net.Client;
using Proto.Command.Client;

namespace Client.Desktop.Communication.Commands.Client;

public class ClientCommandSender : IClientCommandSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly ClientCommandProtoService.ClientCommandProtoServiceClient _client = new(Channel);

    public async Task<bool> Send(ClientCreateTrackingControlCommand command)
    {
        var response = await _client.CreateTimeSlotControlAsync(command.ToProto());
        return response.Success;
    }
}