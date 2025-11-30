using Core.Server.Services.Client;
using Core.Server.Tracing.Tracing.Tracers;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Proto.Command.Client;

namespace Core.Server.Communication.Endpoints.Client;

[Authorize]
public class ClientCommandReceiver(ITrackingControlService trackingControlService, ITraceCollector tracer)
    : ClientCommandProtoService.ClientCommandProtoServiceBase
{
    public override async Task<CommandResponse> CreateTimeSlotControl(CreateTrackingControlCommandProto request,
        ServerCallContext context)
    {
        await tracer.Client.CreateTrackingControl.CommandReceived(GetType(), Guid.Parse(request.TraceData.TraceId),
            request);

        await trackingControlService.Create(Guid.Parse(request.TicketId), request.Date.ToDateTimeOffset(),
            Guid.Parse(request.TraceData.TraceId));

        return new CommandResponse { Success = true };
    }
}