using Grpc.Core;
using Proto.Command.UseCases;
using Service.Server.Old.Services.UseCase;

namespace Service.Server.Communication.Mock.UseCase;

public class UseCaseCommandReceiver(ITimeSlotControlService timeSlotControlService)
    : UseCaseCommandProtoService.UseCaseCommandProtoServiceBase
{
    public override async Task<CommandResponse> CreateTimeSlotControl(CreateTimeSlotControlCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine($"[CreateTimeSlotControl] TicketId: {request.TicketId}");

        await timeSlotControlService.Create(Guid.Parse(request.TicketId));
        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> LoadTimeSlotControl(LoadTimeSlotControlCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine($"[LoadTimeSlotControl] TimeSlotId: {request.TimeSlotId}, ViewId: {request.ViewId}");

        await timeSlotControlService.Create(Guid.Parse(request.TimeSlotId));
        return new CommandResponse { Success = true };
    }
}