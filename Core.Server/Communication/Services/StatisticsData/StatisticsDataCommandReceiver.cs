using Core.Server.Services.Entities.StatisticsData;
using Grpc.Core;
using Proto.Command.StatisticsData;

namespace Core.Server.Communication.Services.StatisticsData;

public class StatisticsDataCommandReceiver(IStatisticsDataCommandsService statisticsDataCommandsService)
    : StatisticsDataCommandProtoService.StatisticsDataCommandProtoServiceBase
{
    public override async Task<CommandResponse> ChangeProductivity(ChangeProductivityCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine(
            $"[ChangeProductivity] ID: {request.StatisticsDataId}, Productive: {request.IsProductive}, Neutral: {request.IsNeutral}, Unproductive: {request.IsUnproductive}");

        await statisticsDataCommandsService.ChangeProductivity(request.ToCommand());
        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> ChangeTagSelection(ChangeTagSelectionCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine(
            $"[ChangeTagSelection] ID: {request.StatisticsDataId}, SelectedTags: {string.Join(", ", request.SelectedTagIds)}");

        await statisticsDataCommandsService.ChangeTagSelection(request.ToCommand());
        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> CreateStatisticsData(CreateStatisticsDataCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine(
            $"[CreateStatisticsData] ID: {request.StatisticsDataId}, Productive: {request.IsProductive}, Neutral: {request.IsNeutral}, Unproductive: {request.IsUnproductive}, Tags: {string.Join(", ", request.TagIds)}, TimeSlotID: {request.TimeSlotId}");

        await statisticsDataCommandsService.Create(request.ToCommand());
        return new CommandResponse { Success = true };
    }
}