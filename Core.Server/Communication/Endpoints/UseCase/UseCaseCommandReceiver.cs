using Core.Server.Services.UseCase;
using Core.Server.Tracing.Tracing.Tracers;
using Grpc.Core;
using Proto.Command.UseCases;

namespace Core.Server.Communication.Endpoints.UseCase;

public class UseCaseCommandReceiver(ITimeSlotControlService timeSlotControlService, ITraceCollector tracer)
    : UseCaseCommandProtoService.UseCaseCommandProtoServiceBase
{
    public override async Task<CommandResponse> CreateTimeSlotControl(CreateTrackingControlCommandProto request,
        ServerCallContext context)
    {

        
        await timeSlotControlService.Create(Guid.Parse(request.TicketId), request.Date.ToDateTimeOffset(),
            Guid.Parse(request.TraceData.TraceId));
        return new CommandResponse { Success = true };
    }
}