using Core.Server.Services.Entities.StatisticsData;
using Core.Server.Tracing.Tracing.Tracers;
using Grpc.Core;
using Proto.Command.StatisticsData;

namespace Core.Server.Communication.Endpoints.StatisticsData;

public class StatisticsDataCommandReceiver(
    IStatisticsDataCommandsService statisticsDataCommandsService,
    ITraceCollector tracer)
    : StatisticsDataCommandProtoService.StatisticsDataCommandProtoServiceBase
{
    public override async Task<CommandResponse> ChangeProductivity(ChangeProductivityCommandProto request,
        ServerCallContext context)
    {
        await tracer.Statistics.ChangeProductivity.CommandReceived(GetType(), Guid.Parse(request.TraceData.TraceId),
            request);

        await statisticsDataCommandsService.ChangeProductivity(request.ToCommand());
        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> ChangeTagSelection(ChangeTagSelectionCommandProto request,
        ServerCallContext context)
    {
        await tracer.Statistics.ChangeTagSelection.CommandReceived(GetType(), Guid.Parse(request.TraceData.TraceId),
            request);

        await statisticsDataCommandsService.ChangeTagSelection(request.ToCommand());
        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> CreateStatisticsData(CreateStatisticsDataCommandProto request,
        ServerCallContext context)
    {
        await tracer.Statistics.Create.CommandReceived(GetType(), Guid.Parse(request.TraceData.TraceId), request);

        await statisticsDataCommandsService.Create(request.ToCommand());
        return new CommandResponse { Success = true };
    }
}