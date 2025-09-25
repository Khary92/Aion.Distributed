using Grpc.Core;
using Proto.Command.UseCases;

namespace Core.Server.Communication.Mocks.UseCase;

public class MockUseCaseCommandService : UseCaseCommandProtoService.UseCaseCommandProtoServiceBase
{
    public override Task<CommandResponse> CreateTimeSlotControl(CreateTrackingControlCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine($"[CreateTimeSlotControl] TicketId: {request.TicketId}");


        return Task.FromResult(new CommandResponse { Success = true });
    }
}