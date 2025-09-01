using Core.Server.Services.Entities.WorkDays;
using Core.Server.Tracing.Tracing.Tracers;
using Grpc.Core;
using Proto.Command.WorkDays;

namespace Core.Server.Communication.Endpoints.WorkDay;

public class WorkDayCommandReceiver(IWorkDayCommandsService workDayCommandsService, ITraceCollector tracer)
    : WorkDayCommandProtoService.WorkDayCommandProtoServiceBase
{
    public override async Task<CommandResponse> CreateWorkDay(CreateWorkDayCommandProto request,
        ServerCallContext context)
    {
        
        await tracer.WorkDay.Create.CommandReceived(GetType(), Guid.Parse(request.TraceData.TraceId), request);

        await workDayCommandsService.Create(request.ToCommand());
        return new CommandResponse { Success = true };
    }
}