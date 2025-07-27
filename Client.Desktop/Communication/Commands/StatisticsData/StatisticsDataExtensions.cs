using Client.Desktop.Communication.Commands.StatisticsData.Records;
using Client.Proto;
using Proto.Command.StatisticsData;

namespace Client.Desktop.Communication.Commands.StatisticsData;

public static class StatisticsDataExtensions
{
    public static CreateStatisticsDataCommandProto ToProto(this ClientCreateStatisticsDataCommand command) => new()
    {
        StatisticsDataId = command.StatisticsDataId.ToString(),
        IsProductive = command.IsProductive,
        TagIds = { command.TagIds.ToRepeatedField() }
    };

    public static ChangeTagSelectionCommandProto ToProto(this ClientChangeTagSelectionCommand command) => new()
    {
        StatisticsDataId = command.StatisticsDataId.ToString(),
        SelectedTagIds= { command.SelectedTagIds.ToRepeatedField() }
    };
    
    public static ChangeProductivityCommandProto ToProto(this ClientChangeProductivityCommand command) => new()
    {
        StatisticsDataId = command.StatisticsDataId.ToString(),
        IsProductive = command.IsProductive,
        IsNeutral = command.IsNeutral,
        IsUnproductive = command.IsUnproductive,
    };
}