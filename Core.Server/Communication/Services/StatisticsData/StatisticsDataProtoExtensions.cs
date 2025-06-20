using Proto.Command.StatisticsData;
using Proto.DTO.StatisticsData;
using Proto.Notifications.StatisticsData;
using Service.Server.Communication.CQRS.Commands.Entities.StatisticsData;

namespace Service.Server.Communication.Services.StatisticsData;

public static class StatisticsDataProtoExtensions
{
    public static ChangeProductivityCommand ToCommand(
        this ChangeProductivityCommandProto proto) =>
        new(Guid.Parse(proto.StatisticsDataId), proto.IsProductive, proto.IsNeutral, proto.IsUnproductive);


    public static StatisticsDataNotification ToNotification(this ChangeProductivityCommand command) =>
        new()
        {
            ChangeProductivity = new ChangeProductivityNotification()
            {
                StatisticsDataId = command.StatisticsDataId.ToString(),
                IsProductive = command.IsProductive,
                IsNeutral = command.IsNeutral,
                IsUnproductive = command.IsUnproductive
            }
        };

    public static ChangeTagSelectionCommand ToCommand(
        this ChangeTagSelectionCommandProto proto) =>
        new(Guid.Parse(proto.StatisticsDataId), proto.SelectedTagIds.ToGuidList());


    public static StatisticsDataNotification ToNotification(this ChangeTagSelectionCommand command) =>
        new()
        {
            ChangeTagSelection = new ChangeTagSelectionNotification()
            {
                StatisticsDataId = command.StatisticsDataId.ToString(),
                SelectedTagIds = { command.SelectedTagIds.ToRepeatedField() }
            }
        };

    public static CreateStatisticsDataCommand ToCommand(
        this CreateStatisticsDataCommandProto proto) =>
        new(Guid.Parse(proto.StatisticsDataId), proto.IsProductive, proto.IsNeutral, proto.IsUnproductive,
            proto.TagIds.ToGuidList(), Guid.Parse(proto.TimeSlotId));


    public static StatisticsDataNotification ToNotification(this CreateStatisticsDataCommand command) =>
        new()
        {
            StatisticsDataCreated = new StatisticsDataCreatedNotification
            {
                StatisticsDataId = command.StatisticsDataId.ToString(),
                IsProductive = command.IsProductive,
                IsNeutral = command.IsNeutral,
                IsUnproductive = command.IsUnproductive,
                TagIds = {command.TagIds.ToRepeatedField()},
                TimeSlotId = command.TimeSlotId.ToString()
            }
        };


    public static StatisticsDataProto ToProto(this Domain.Entities.StatisticsData statisticsData) =>
        new()
        {
            StatisticsId = statisticsData.StatisticsId.ToString(),
            IsProductive = statisticsData.IsProductive,
            IsNeutral = statisticsData.IsNeutral,
            IsUnproductive = statisticsData.IsUnproductive,
            TagIds = {statisticsData.TagIds.ToRepeatedField()},
            TimeSlotId = statisticsData.TimeSlotId.ToString()
        };
}