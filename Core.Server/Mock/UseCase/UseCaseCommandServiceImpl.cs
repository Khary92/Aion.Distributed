using System;
using System.Threading.Tasks;
using Grpc.Core;
using Proto.Command.UseCases;

namespace Service.Server.Mock;

public class UseCaseCommandServiceImpl : UseCaseCommandService.UseCaseCommandServiceBase
{
    public override Task<CommandResponse> CreateTimeSlotControl(CreateTimeSlotControlCommand request, ServerCallContext context)
    {
        Console.WriteLine($"[CreateTimeSlotControl] TicketId: {request.TicketId}, ViewId: {request.ViewId}");

        // Platzhalter für echte Logik
        return Task.FromResult(new CommandResponse { Success = true });
    }

    public override Task<CommandResponse> LoadTimeSlotControl(LoadTimeSlotControlCommand request, ServerCallContext context)
    {
        Console.WriteLine($"[LoadTimeSlotControl] TimeSlotId: {request.TimeSlotId}, ViewId: {request.ViewId}");

        // Platzhalter für echte Logik
        return Task.FromResult(new CommandResponse { Success = true });
    }
}