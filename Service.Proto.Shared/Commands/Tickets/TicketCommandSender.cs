using Grpc.Net.Client;
using Proto.Command.Tickets;

namespace Service.Proto.Shared.Commands.Tickets;

public class TicketCommandSender : ITicketCommandSender
{
    private readonly TicketCommandProtoService.TicketCommandProtoServiceClient _client;

    public TicketCommandSender(string address)
    {
        var channel = GrpcChannel.ForAddress(address);
        _client = new TicketCommandProtoService.TicketCommandProtoServiceClient(channel);
    }

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