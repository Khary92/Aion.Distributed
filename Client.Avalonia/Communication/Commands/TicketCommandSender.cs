using System.Threading.Tasks;
using Grpc.Net.Client;
using Proto.Command.Tickets;

namespace Client.Avalonia.Communication.Sender;

public class TicketCommandSender : ITicketCommandSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempStatic.Address);
    private readonly TicketCommandService.TicketCommandServiceClient _client = new(Channel);

    public async Task<bool> Send(CreateTicketCommand command)
    {
        var response = await _client.CreateTicketAsync(command);
        return response.Success;
    }

    public async Task<bool> Send(UpdateTicketDataCommand command)
    {
        var response = await _client.UpdateTicketDataAsync(command);
        return response.Success;
    }

    public async Task<bool> Send(UpdateTicketDocumentationCommand command)
    {
        var response = await _client.UpdateTicketDocumentationAsync(command);
        return response.Success;
    }
}