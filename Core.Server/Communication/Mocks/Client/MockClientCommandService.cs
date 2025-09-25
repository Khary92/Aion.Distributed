using Grpc.Core;
using Proto.Command.Client;

namespace Core.Server.Communication.Mocks.Client;

public class MockClientCommandService : ClientCommandProtoService.ClientCommandProtoServiceBase
{
    public override Task<CommandResponse> CreateTimeSlotControl(CreateTrackingControlCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine($"[CreateTimeSlotControl] TicketId: {request.TicketId}");


        return Task.FromResult(new CommandResponse { Success = true });
    }
}