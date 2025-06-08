using System.Threading.Tasks;
using Grpc.Core;
using Proto.Command;

namespace Client.Avalonia.Communication.Sender;

public class CommandServiceImpl : CommandService.CommandServiceBase
{
    public override Task<CommandResponse> SendCommand(CreateTicketCommand request, ServerCallContext context)
    {
        var response = new CommandResponse
        {
            Success = true
        };

        return Task.FromResult(response);
    }
}