using System.Threading.Tasks;
using Grpc.Net.Client;
using Proto.Client;
using Proto.Command.Tickets;

namespace Client.Desktop.Communication.Commands.Tickets;

public class TicketCommandSender : ITicketCommandSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly TicketCommandProtoService.TicketCommandProtoServiceClient _client = new(Channel);

    public async Task<bool> Send(CreateTicketCommandProto command)
    {
        var response = await _client.CreateTicketAsync(command);
        return response.Success;
    }

    public async Task<bool> Send(UpdateTicketDataCommandProto command)
    {
        var response = await _client.UpdateTicketDataAsync(command);
        return response.Success;
    }

    public async Task<bool> Send(UpdateTicketDocumentationCommandProto command)
    {
        var response = await _client.UpdateTicketDocumentationAsync(command);
        return response.Success;
    }
}