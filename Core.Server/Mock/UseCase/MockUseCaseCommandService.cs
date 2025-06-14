using Grpc.Core;
using Proto.Command.UseCases;

namespace Service.Server.Mock.UseCase;

public class MockUseCaseCommandService : UseCaseCommandService.UseCaseCommandServiceBase
{
    public override Task<CommandResponse> CreateTimeSlotControl(CreateTimeSlotControlCommand request, ServerCallContext context)
    {
        Console.WriteLine($"[CreateTimeSlotControl] TicketId: {request.TicketId}");

        
        
        return Task.FromResult(new CommandResponse { Success = true });
    }

    public override Task<CommandResponse> LoadTimeSlotControl(LoadTimeSlotControlCommand request, ServerCallContext context)
    {
        Console.WriteLine($"[LoadTimeSlotControl] TimeSlotId: {request.TimeSlotId}, ViewId: {request.ViewId}");

        // Platzhalter für echte Logik
        return Task.FromResult(new CommandResponse { Success = true });
    }
}