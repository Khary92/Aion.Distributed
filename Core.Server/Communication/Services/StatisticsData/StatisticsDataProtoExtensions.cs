using Core.Server.Communication.CQRS.Commands.Entities.StatisticsData;
using Proto.Command.StatisticsData;
using Proto.DTO.StatisticsData;
using Proto.Notifications.StatisticsData;
using Proto.Shared;

namespace Core.Server.Communication.Services.StatisticsData;

public static class StatisticsDataProtoExtensions
{
    public static ChangeProductivityCommand ToCommand(
        this ChangeProductivityCommandProto proto)
    {
        return new ChangeProductivityCommand(Guid.Parse(proto.StatisticsDataId), proto.IsProductive, proto.IsNeutral,
            proto.IsUnproductive);
    }


    public static StatisticsDataNotification ToNotification(this ChangeProductivityCommand command)
    {
        return new StatisticsDataNotification
        {
            ChangeProductivity = new ChangeProductivityNotification
            {
                StatisticsDataId = command.StatisticsDataId.ToString(),
                IsProductive = command.IsProductive,
                IsNeutral = command.IsNeutral,
                IsUnproductive = command.IsUnproductive
            }
        };
    }

    public static ChangeTagSelectionCommand ToCommand(
        this ChangeTagSelectionCommandProto proto)
    {
        return new ChangeTagSelectionCommand(Guid.Parse(proto.StatisticsDataId), proto.SelectedTagIds.ToGuidList());
    }


    public static StatisticsDataNotification ToNotification(this ChangeTagSelectionCommand command)
    {
        return new StatisticsDataNotification
        {
            ChangeTagSelection = new ChangeTagSelectionNotification
            {
                StatisticsDataId = command.StatisticsDataId.ToString(),
                SelectedTagIds = { command.SelectedTagIds.ToRepeatedField() }
            }
        };
    }

    public static CreateStatisticsDataCommand ToCommand(
        this CreateStatisticsDataCommandProto proto)
    {
        return new CreateStatisticsDataCommand(Guid.Parse(proto.StatisticsDataId), proto.IsProductive, proto.IsNeutral,
            proto.IsUnproductive,
            proto.TagIds.ToGuidList(), Guid.Parse(proto.TimeSlotId));
    }


    public static StatisticsDataNotification ToNotification(this CreateStatisticsDataCommand command)
    {
        return new StatisticsDataNotification
        {
            StatisticsDataCreated = new StatisticsDataCreatedNotification
            {
                StatisticsDataId = command.StatisticsDataId.ToString(),
                IsProductive = command.IsProductive,
                IsNeutral = command.IsNeutral,
                IsUnproductive = command.IsUnproductive,
                TagIds = { command.TagIds.ToRepeatedField() },
                TimeSlotId = command.TimeSlotId.ToString()
            }
        };
    }


    public static StatisticsDataProto ToProto(this Domain.Entities.StatisticsData statisticsData)
    {
        return new StatisticsDataProto
        {
            StatisticsId = statisticsData.StatisticsId.ToString(),
            IsProductive = statisticsData.IsProductive,
            IsNeutral = statisticsData.IsNeutral,
            IsUnproductive = statisticsData.IsUnproductive,
            TagIds = { statisticsData.TagIds.ToRepeatedField() },
            TimeSlotId = statisticsData.TimeSlotId.ToString()
        };
    }
}