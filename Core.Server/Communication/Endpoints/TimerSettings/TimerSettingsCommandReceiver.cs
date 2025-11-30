using Core.Server.Services.Entities.TimerSettings;
using Core.Server.Tracing.Tracing.Tracers;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Proto.Command.TimerSettings;

namespace Core.Server.Communication.Endpoints.TimerSettings;

[Authorize]
public class TimerSettingsCommandReceiver(
    ITimerSettingsCommandsService timerSettingsCommandsService,
    ITraceCollector tracer)
    : TimerSettingsCommandProtoService.TimerSettingsCommandProtoServiceBase
{
    public override async Task<CommandResponse> ChangeDocuTimerSaveInterval(
        ChangeDocuTimerSaveIntervalCommandProto request, ServerCallContext context)
    {
        await tracer.TimerSettings.ChangeDocuTimerInterval.CommandReceived(GetType(),
            Guid.Parse(request.TraceData.TraceId), request);

        await timerSettingsCommandsService.ChangeDocumentationInterval(request.ToCommand());
        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> ChangeSnapshotSaveInterval(
        ChangeSnapshotSaveIntervalCommandProto request, ServerCallContext context)
    {
        await tracer.TimerSettings.ChangeSnapshotInterval.CommandReceived(GetType(),
            Guid.Parse(request.TraceData.TraceId), request);

        await timerSettingsCommandsService.ChangeSnapshotInterval(request.ToCommand());
        return new CommandResponse { Success = true };
    }
}