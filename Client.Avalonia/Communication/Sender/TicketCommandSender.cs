using System.Threading.Tasks;
using Grpc.Net.Client;
using Proto.Command.Tickets;

public class TicketCommandSender
{
    private static readonly string Address = "https://localhost:5001";
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(Address);
    private readonly TicketCommandService.TicketCommandServiceClient _client = new(Channel);

    public async Task<bool> Send(CreateTicketCommand createTicketCommand)
    {
        var response = await _client.CreateTicketAsync(createTicketCommand);
        return response.Success;
    }

    public async Task<bool> Send(UpdateTicketDataCommand updateTicketDataCommand)
    {
        var response = await _client.UpdateTicketDataAsync(updateTicketDataCommand);
        return response.Success;
    }

    public async Task<bool> Send(UpdateTicketDocumentationCommand updateTicketDocumentationCommand)
    {
        var response = await _client.UpdateTicketDocumentationAsync(updateTicketDocumentationCommand);
        return response.Success;
    }
}