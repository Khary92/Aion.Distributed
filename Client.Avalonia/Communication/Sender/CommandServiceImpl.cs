using System.Threading.Tasks;
using Grpc.Net.Client;
using Proto.Command;

namespace Client.Avalonia.Communication.Sender;

public class CommandServiceImpl : CommandService.CommandServiceBase
{
    public async Task Send(CreateTicketCommand createTicketCommand)
    {
        var channel = GrpcChannel.ForAddress("https://localhost:5001"); // oder dein Zielhost
        var client = new CommandService.CommandServiceClient(channel);

        var response = await client.SendCommandAsync(createTicketCommand);
    }
}