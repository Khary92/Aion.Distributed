using Client.Proto;
using Core.Server.Communication.Records.Commands.Entities.StatisticsData;
using Proto.Command.StatisticsData;
using Proto.DTO.StatisticsData;
using Proto.Notifications.StatisticsData;

namespace Core.Server.Communication.Endpoints.StatisticsData;

public static class StatisticsDataProtoExtensions
{
    public static ChangeProductivityCommand ToCommand(
        this ChangeProductivityCommandProto proto) => new(Guid.Parse(proto.StatisticsDataId), proto.IsProductive,
        proto.IsNeutral, proto.IsUnproductive, Guid.Parse(proto.TraceData.TraceId));


    public static StatisticsDataNotification ToNotification(this ChangeProductivityCommand command) => new()
    {
        ChangeProductivity = new ChangeProductivityNotification
        {
            StatisticsDataId = command.StatisticsDataId.ToString(),
            IsProductive = command.IsProductive,
            IsNeutral = command.IsNeutral,
            IsUnproductive = command.IsUnproductive,
            TraceData = new()
            {
                TraceId = command.TraceId.ToString()
            }
        }
    };

    public static ChangeTagSelectionCommand ToCommand(this ChangeTagSelectionCommandProto proto) => new(
        Guid.Parse(proto.StatisticsDataId), proto.SelectedTagIds.ToGuidList(), Guid.Parse(proto.TraceData.TraceId));


    public static StatisticsDataNotification ToNotification(this ChangeTagSelectionCommand command) => new()
    {
        ChangeTagSelection = new ChangeTagSelectionNotification
        {
            StatisticsDataId = command.StatisticsDataId.ToString(),
            SelectedTagIds = { command.SelectedTagIds.ToRepeatedField() },
            TraceData = new()
            {
                TraceId = command.TraceId.ToString()
            }
        }
    };

    public static CreateStatisticsDataCommand ToCommand(this CreateStatisticsDataCommandProto proto) => new(
        Guid.Parse(proto.StatisticsDataId), proto.IsProductive, proto.IsNeutral,
        proto.IsUnproductive, proto.TagIds.ToGuidList(), Guid.Parse(proto.TimeSlotId),
        Guid.Parse(proto.TraceData.TraceId));


    public static StatisticsDataNotification ToNotification(this CreateStatisticsDataCommand command) => new()
    {
        StatisticsDataCreated = new StatisticsDataCreatedNotification
        {
            StatisticsDataId = command.StatisticsDataId.ToString(),
            IsProductive = command.IsProductive,
            IsNeutral = command.IsNeutral,
            IsUnproductive = command.IsUnproductive,
            TagIds = { command.TagIds.ToRepeatedField() },
            TimeSlotId = command.TimeSlotId.ToString(),
            TraceData = new()
            {
                TraceId = command.TraceId.ToString()
            }
        }
    };
    
    public static StatisticsDataProto ToProto(this Domain.Entities.StatisticsData statisticsData) => new()
    {
        StatisticsId = statisticsData.StatisticsId.ToString(),
        IsProductive = statisticsData.IsProductive,
        IsNeutral = statisticsData.IsNeutral,
        IsUnproductive = statisticsData.IsUnproductive,
        TagIds = { statisticsData.TagIds.ToRepeatedField() },
        TimeSlotId = statisticsData.TimeSlotId.ToString()
    };
}