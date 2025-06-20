using Grpc.Core;
using Proto.Command.UseCases;

namespace Core.Server.Communication.Mock.UseCase;

public class MockUseCaseCommandService : UseCaseCommandProtoService.UseCaseCommandProtoServiceBase
{
    public override Task<CommandResponse> CreateTimeSlotControl(CreateTimeSlotControlCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine($"[CreateTimeSlotControl] TicketId: {request.TicketId}");


        return Task.FromResult(new CommandResponse { Success = true });
    }

    public override Task<CommandResponse> LoadTimeSlotControl(LoadTimeSlotControlCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine($"[LoadTimeSlotControl] TimeSlotId: {request.TimeSlotId}, ViewId: {request.ViewId}");

        return Task.FromResult(new CommandResponse { Success = true });
    }
}