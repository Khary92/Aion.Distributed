using Grpc.Core;
using Proto.Command.StatisticsData;
using Proto.Notifications.StatisticsData;
using Service.Server.Communication.StatisticsData;

namespace Service.Server.Communication.Mock.StatisticsData;

public class MockStatisticsDataCommandService(StatisticsDataNotificationServiceImpl statisticsDataNotificationService)
    : StatisticsDataCommandProtoService.StatisticsDataCommandProtoServiceBase
{
    public override async Task<CommandResponse> ChangeProductivity(ChangeProductivityCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine(
            $"[ChangeProductivity] ID: {request.StatisticsDataId}, Productive: {request.IsProductive}, Neutral: {request.IsNeutral}, Unproductive: {request.IsUnproductive}");

        try
        {
            await statisticsDataNotificationService.SendNotificationAsync(new StatisticsDataNotification
            {
                ChangeProductivity = new ChangeProductivityNotification
                {
                    StatisticsDataId = request.StatisticsDataId,
                    IsProductive = request.IsProductive,
                    IsNeutral = request.IsNeutral,
                    IsUnproductive = request.IsUnproductive
                }
            });

            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] ChangeProductivity failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }

    public override async Task<CommandResponse> ChangeTagSelection(ChangeTagSelectionCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine(
            $"[ChangeTagSelection] ID: {request.StatisticsDataId}, SelectedTags: {string.Join(", ", request.SelectedTagIds)}");

        try
        {
            await statisticsDataNotificationService.SendNotificationAsync(new StatisticsDataNotification
            {
                ChangeTagSelection = new ChangeTagSelectionNotification()
                {
                    StatisticsDataId = request.StatisticsDataId,
                    SelectedTagIds = { request.SelectedTagIds }
                }
            });

            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] ChangeTagSelection failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }

    public override async Task<CommandResponse> CreateStatisticsData(CreateStatisticsDataCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine(
            $"[CreateStatisticsData] ID: {request.StatisticsDataId}, Productive: {request.IsProductive}, Neutral: {request.IsNeutral}, Unproductive: {request.IsUnproductive}, Tags: {string.Join(", ", request.TagIds)}, TimeSlotID: {request.TimeSlotId}");

        try
        {
            await statisticsDataNotificationService.SendNotificationAsync(new StatisticsDataNotification
            {
                StatisticsDataCreated = new StatisticsDataCreatedNotification
                {
                    StatisticsDataId = request.StatisticsDataId,
                    IsProductive = request.IsProductive,
                    IsNeutral = request.IsNeutral,
                    IsUnproductive = request.IsUnproductive,
                    TagIds = { request.TagIds },
                    TimeSlotId = request.TimeSlotId
                }
            });

            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] CreateStatisticsData failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }
}