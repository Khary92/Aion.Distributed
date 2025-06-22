using Core.Server.Services.UseCase;
using Grpc.Core;
using Proto.Command.UseCases;

namespace Core.Server.Communication.Endpoints.UseCase;

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
}